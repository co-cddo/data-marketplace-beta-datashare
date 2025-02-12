using Agrimetrics.DataShare.Api.Controllers.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Requests.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.AuditLogs
{
    [TestFixture]
    public class AuditLogResponseFactoryTests
    {
        #region CreateGetDataShareRequestAuditLogResponse() Tests
        [Test]
        public void GivenANullGetDataShareRequestAuditLogRequest_WhenICreateGetDataShareRequestAuditLogResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AuditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
                    null!,
                    testItems.Fixture.Create<IAuditLogDataShareRequestStatusChangesResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestAuditLogRequest"));
        }

        [Test]
        public void GivenANullAuditLogDataShareRequestStatusChangesResult_WhenICreateGetDataShareRequestAuditLogResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AuditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
                    testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("auditLogDataShareRequestStatusChangesResult"));
        }

        [Test]
        public void GivenAGetDataShareRequestAuditLogRequest_WhenICreateGetDataShareRequestAuditLogResponse_ThenAGetDataShareRequestAuditLogResponseIsCreatedUsingTheGetDataShareRequestAuditLogRequest()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

            var result = testItems.AuditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
                testGetDataShareRequestAuditLogRequest,
                testItems.Fixture.Create<IAuditLogDataShareRequestStatusChangesResult>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testGetDataShareRequestAuditLogRequest.DataShareRequestId));
                Assert.That(result.ToStatuses, Is.EqualTo(testGetDataShareRequestAuditLogRequest.ToStatuses));
            });
        }

        [Test]
        public void GivenAnAuditLogDataShareRequestStatusChangesResult_WhenICreateGetDataShareRequestAuditLogResponse_ThenAGetDataShareRequestAuditLogResponseIsCreatedUsingTheAuditLogDataShareRequestStatusChangesResult()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestAuditLog = testItems.Fixture.Create<DataShareRequestAuditLog>();

            var mockAuditLogDataShareRequestStatusChangesResult = new Mock<IAuditLogDataShareRequestStatusChangesResult>();
            mockAuditLogDataShareRequestStatusChangesResult.Setup(x => x.DataShareRequestAuditLog)
                .Returns(() => testDataShareRequestAuditLog);

            var result = testItems.AuditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
                testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>(),
                mockAuditLogDataShareRequestStatusChangesResult.Object);

            Assert.That(result.DataShareRequestAuditLog, Is.EqualTo(testDataShareRequestAuditLog));
        }
        #endregion
        
        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var auditLogResponseFactory = new AuditLogResponseFactory();

            return new TestItems(fixture, auditLogResponseFactory);
        }

        private class TestItems(
            IFixture fixture,
            IAuditLogResponseFactory auditLogResponseFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IAuditLogResponseFactory AuditLogResponseFactory { get; } = auditLogResponseFactory;
        }
        #endregion
    }
}
