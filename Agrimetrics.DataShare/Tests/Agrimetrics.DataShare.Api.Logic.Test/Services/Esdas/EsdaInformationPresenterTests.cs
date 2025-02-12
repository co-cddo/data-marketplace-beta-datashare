using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Esdas
{
    [TestFixture]
    public class EsdaInformationPresenterTests
    {
        #region GetEsdaDetailsByIdAsync() Tests
        [Test]
        [TestCaseSource(nameof(EsdaDetailsTestCaseData))]
        public async Task GivenAKnownEsdaId_WhenIGetEsdaDetailsByIdAsync_ThenTheDetailsOfTheEsdaAreReturned(
            Guid testEsdaId,
            string testTitle,
            int testOrganisationId,
            int testDomainId,
            string testContactPointName,
            string testContactPointEmailAddress,
            DataShareRequestNotificationRecipientType? testDataShareRequestNotificationRecipientType,
            string? testCustomDsrNotificationAddress)
        {
            var testItems = CreateTestItems();

            testItems.MockDataSetPresenter.Setup(x => x.GetEsdaOwnershipDetailsAsync(testEsdaId))
                .ReturnsAsync(() =>
                    new GetEsdaOwnershipDetailsResponse
                    {
                        EsdaId = testEsdaId,
                        Title = testTitle,
                        OrganisationId = testOrganisationId,
                        DomainId = testDomainId,
                        ContactPointName = testContactPointName,
                        ContactPointEmailAddress = testContactPointEmailAddress,
                        DataShareRequestNotificationRecipientType = testDataShareRequestNotificationRecipientType,
                        CustomDsrNotificationAddress = testCustomDsrNotificationAddress
                    });

            var result = await testItems.EsdaInformationPresenter.GetEsdaDetailsByIdAsync(testEsdaId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(testEsdaId));
                Assert.That(result.Title, Is.EqualTo(testTitle));
                Assert.That(result.SupplierOrganisationId, Is.EqualTo(testOrganisationId));
                Assert.That(result.SupplierDomainId, Is.EqualTo(testDomainId));
                Assert.That(result.ContactPointName, Is.EqualTo(testContactPointName));
                Assert.That(result.ContactPointEmailAddress, Is.EqualTo(testContactPointEmailAddress));
                Assert.That(result.DataShareRequestNotificationRecipientType, Is.EqualTo(testDataShareRequestNotificationRecipientType));
                Assert.That(result.CustomDsrNotificationAddress, Is.EqualTo(testCustomDsrNotificationAddress));
            });
        }

        private static IEnumerable<TestCaseData> EsdaDetailsTestCaseData()
        {
            var fixture = new Fixture();

            var testEsdaId = fixture.Create<Guid>();
            const string testTitle = "test esda title";
            const int testOrganisationId = 123;
            const int testDomainId = 456;
            const string testContactPointName = "test maintainer";
            const string testContactPointEmailAddress = "test maintainer email";

            var testDataShareRequestNotificationRecipientTypes = 
                Enum.GetValues<DataShareRequestNotificationRecipientType>().Cast<DataShareRequestNotificationRecipientType?>()
                .Concat([null]);

            var testCustomDsrNotificationAddresses = new List<string?>{null, "", "   ", "custom dsr notification address"};

            foreach (var testDataShareRequestNotificationRecipientType in testDataShareRequestNotificationRecipientTypes)
            {
                foreach (var testCustomDsrNotificationAddress in testCustomDsrNotificationAddresses)
                {
                    yield return new TestCaseData(
                        testEsdaId,
                        testTitle,
                        testOrganisationId,
                        testDomainId,
                        testContactPointName,
                        testContactPointEmailAddress,
                        testDataShareRequestNotificationRecipientType,
                        testCustomDsrNotificationAddress);
                }
            }
        }

        [Test]
        public async Task GivenAnExceptionIsThrownGettingTheEsdaOwnershipDetails_WhenIGetEsdaDetailsByIdAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            var testException = new Exception("test exception message");
            testItems.MockDataSetPresenter.Setup(x => x.GetEsdaOwnershipDetailsAsync(testEsdaId))
                .Throws(testException);

            await Task.Run(() => Assert.ThrowsAsync<DataShareRequestGeneralException>(async () => await testItems.EsdaInformationPresenter.GetEsdaDetailsByIdAsync(testEsdaId)));

            testItems.MockLogger.VerifyLog(
                LogLevel.Error,
                $"Failed to get details of ESDA with Id '{testEsdaId}'",
                testException);
        }

        [Test] public void GivenAnExceptionIsThrownGettingTheEsdaOwnershipDetails_WhenIGetEsdaDetailsByIdAsync_ThenTheExceptionIsReThrown()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            var testException = new Exception("test exception message");
            testItems.MockDataSetPresenter.Setup(x => x.GetEsdaOwnershipDetailsAsync(testEsdaId))
                .Throws(testException);

            var resultException = Assert.ThrowsAsync<DataShareRequestGeneralException>(async () => await testItems.EsdaInformationPresenter.GetEsdaDetailsByIdAsync(testEsdaId));

            Assert.Multiple(() =>
            {
                Assert.That(resultException!.Message, Is.EqualTo($"Failed to get details of ESDA with Id '{testEsdaId}'"));
                Assert.That(resultException!.InnerException, Is.SameAs(testException));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockLogger = new Mock<ILogger<EsdaInformationPresenter>>();
            var mockDataSetPresenter = new Mock<IDataAssetPresenter>();

            var esdaInformationPresenter = new EsdaInformationPresenter(
                mockLogger.Object,
                mockDataSetPresenter.Object);

            return new TestItems(
                fixture,
                esdaInformationPresenter,
                mockLogger,
                mockDataSetPresenter);
        }

        private class TestItems(
            IFixture fixture,
            IEsdaInformationPresenter esdaInformationPresenter,
            Mock<ILogger<EsdaInformationPresenter>> mockLogger,
            Mock<IDataAssetPresenter> mockDataSetPresenter)
        {
            public IFixture Fixture { get; } = fixture;
            public IEsdaInformationPresenter EsdaInformationPresenter { get; } = esdaInformationPresenter;
            public Mock<ILogger<EsdaInformationPresenter>> MockLogger { get; } = mockLogger;
            public Mock<IDataAssetPresenter> MockDataSetPresenter { get; } = mockDataSetPresenter;
        }
        #endregion
    }
}
