using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;
using AutoFixture.AutoMoq;
using AutoFixture;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Esdas.DataSetPresentation
{
    [TestFixture]
    public class DataAssetPresenterTests
    {
        #region GetEsdaOwnershipDetailsAsync()
        [Test]
        public async Task GivenAConfiguredGetEsdaOwnershipDetailsEndPoint_WhenIGetEsdaOwnershipDetailsAsync_ThenTheQueryToGetEsdaOwnershipDetailsIsMadeToTheConfiguredEndpoint()
        {
            var testItems = CreateTestItems();

            const string testOAuthToken = "123456789";
            const string testGetEsdaOwnershipDetailsEndPoint = "http://testendpoint";
            
            testItems.MockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken()).Returns(testOAuthToken);

            testItems.MockDatasetInformationServiceConfigurationPresenter
                .Setup(x => x.GetEsdaOwnershipDetailsEndPoint())
                .Returns(testGetEsdaOwnershipDetailsEndPoint);

            var testEsdaId = Guid.Parse("F1C34112-1290-4FEE-99A8-91EB032ABCCC");

            var testResponse = testItems.Fixture.Create<GetEsdaOwnershipDetailsResponse>();

            using var httpTest = new HttpTest();
            httpTest.RespondWith(JsonSerializer.Serialize(testResponse));

            await testItems.DataAssetPresenter.GetEsdaOwnershipDetailsAsync(testEsdaId);

            httpTest.ShouldHaveCalled(testGetEsdaOwnershipDetailsEndPoint)
                .WithQueryParam("DataAssetId", testEsdaId.ToString())
                .WithOAuthBearerToken(testOAuthToken);
        }

        [Test]
        public async Task GivenAReturnedEsdaOwnershipDetailsModel_WhenIGetEsdaOwnershipDetailsAsync_ThenTheReturnedEsdaOwnershipDetailsModelIsReturned()
        {
            var testItems = CreateTestItems();

            var testResponse = testItems.Fixture.Create<GetEsdaOwnershipDetailsResponse>();

            using var httpTest = new HttpTest();
            httpTest.RespondWith(JsonSerializer.Serialize(testResponse));

            var result = await testItems.DataAssetPresenter.GetEsdaOwnershipDetailsAsync(It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.EsdaId, Is.EqualTo(testResponse.EsdaId));
                Assert.That(result.Title, Is.EqualTo(testResponse.Title));
                Assert.That(result.OrganisationId, Is.EqualTo(testResponse.OrganisationId));
                Assert.That(result.DomainId, Is.EqualTo(testResponse.DomainId));
                Assert.That(result.ContactPointName, Is.EqualTo(testResponse.ContactPointName));
                Assert.That(result.ContactPointEmailAddress, Is.EqualTo(testResponse.ContactPointEmailAddress));
                Assert.That(result.DataShareRequestNotificationRecipientType, Is.EqualTo(testResponse.DataShareRequestNotificationRecipientType));
                Assert.That(result.CustomDsrNotificationAddress, Is.EqualTo(testResponse.CustomDsrNotificationAddress));
            });
        }

        [Test]
        public void GivenCallingTheGetEsdaOwnershipDetailsEndpointWillThrowAnException_WhenIGetEsdaOwnershipDetailsAsync_ThenADataSetFetchExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            using var httpTest = new HttpTest();

            httpTest.RespondWith("test error", 401);

            var exception = Assert.ThrowsAsync<DataSetFetchException>(async () => await testItems.DataAssetPresenter.GetEsdaOwnershipDetailsAsync(It.IsAny<Guid>()))!;

            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(401));
                Assert.That(exception.ResponseText, Is.EqualTo("test error"));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockLogger = Mock.Get(fixture.Freeze<ILogger<DataAssetPresenter>>());
            var mockUserIdPresenter = Mock.Get(fixture.Freeze<IUserIdPresenter>());
            var mockDatasetInformationServiceConfigurationPresenter = Mock.Get(fixture.Freeze<IDataAssetInformationServiceConfigurationPresenter>());

            var dataSetPresenter = new DataAssetPresenter(
                mockLogger.Object,
                mockUserIdPresenter.Object,
                mockDatasetInformationServiceConfigurationPresenter.Object);

            ConfigureHappyPathTesting();

            return new TestItems(
                fixture,
                dataSetPresenter,
                mockUserIdPresenter,
                mockDatasetInformationServiceConfigurationPresenter);

            void ConfigureHappyPathTesting()
            {
                mockUserIdPresenter.Setup(x => x.GetInitiatingUserIdToken())
                    .Returns(fixture.Create<string>());

                mockDatasetInformationServiceConfigurationPresenter.Setup(x => x.GetEsdaOwnershipDetailsEndPoint())
                    .Returns("http://abc");
            }
        }

        private class TestItems(
            IFixture fixture,
            IDataAssetPresenter dataAssetPresenter,
            Mock<IUserIdPresenter> mockUserIdPresenter,
            Mock<IDataAssetInformationServiceConfigurationPresenter> mockDatasetInformationServiceConfigurationPresenter)
        {
            public IFixture Fixture { get; } = fixture;
            public IDataAssetPresenter DataAssetPresenter { get; } = dataAssetPresenter;
            public Mock<IUserIdPresenter> MockUserIdPresenter { get; } = mockUserIdPresenter;
            public Mock<IDataAssetInformationServiceConfigurationPresenter> MockDatasetInformationServiceConfigurationPresenter { get; } = mockDatasetInformationServiceConfigurationPresenter;
        }
        #endregion
    }
}
