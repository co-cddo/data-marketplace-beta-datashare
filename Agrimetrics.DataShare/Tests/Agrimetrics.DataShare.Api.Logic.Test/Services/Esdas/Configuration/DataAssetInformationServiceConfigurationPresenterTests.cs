using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Esdas.Configuration
{
    [TestFixture]
    public class DataAssetInformationServiceConfigurationPresenterTests
    {
        #region GetDataAssetByIdEndPoint() Tests
        [Test]
        public void GivenServiceConfiguration_WhenIGetDataAssetByIdEndPoint_ThenTheDataAssetByIdEndPointIsWithinTheUtilityApiAddressSpace()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> {"ExternalServices", "UtilityApi"}, "api_address"))
                .Returns(() => "Test Api Address");

            var result = testItems.DataAssetInformationServiceConfigurationPresenter.GetDataAssetByIdEndPoint();

            Assert.That(result, Is.EqualTo("Test Api Address/DataAsset/get-cddo-data-asset"));
        }
        #endregion

        #region GetEsdaOwnershipDetailsEndPoint() Tests
        [Test]
        public void GivenServiceConfiguration_WhenIGetEsdaOwnershipDetailsEndPoint_ThenTheDataSetByIdEndPointIsWithinTheUtilityApiAddressSpace()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UtilityApi" }, "api_address"))
                .Returns(() => "Test Api Address");

            var result = testItems.DataAssetInformationServiceConfigurationPresenter.GetEsdaOwnershipDetailsEndPoint();

            Assert.That(result, Is.EqualTo("Test Api Address/DataAsset/get-esda-ownership-details"));
        }
        #endregion

        #region GetAllSettingValues()
        [Test]
        public void GivenServiceConfiguration_WhenIGetAllSettings_ThenADescriptionOfAllSettingsIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UtilityApi" }, "api_address"))
                .Returns(() => "Test Api Address");

            var result = testItems.DataAssetInformationServiceConfigurationPresenter.GetAllSettings().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(2).Items);

                Assert.That(result.Any(x =>
                        x is { Description: "Get Data Asset By Id EndPoint", Value: "Test Api Address/DataAsset/get-cddo-data-asset" }),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is { Description: "Get Esda Ownership Details Endpoint", Value: "Test Api Address/DataAsset/get-esda-ownership-details" }),
                    Is.True);
            });

        }

        [Test]
        public void GivenGettingAConfigurationValueWithThrowAnException_WhenIGetAllSettingValues_ThenAnErrorIsReturnedForThatValue()
        {
            var testItems = CreateTestItems();

            testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInMultiLevelSection(
                    new List<string> { "ExternalServices", "UtilityApi" }, "api_address"))
                .Throws(new Exception("test exception message"));

            var result = testItems.DataAssetInformationServiceConfigurationPresenter.GetAllSettings().ToList();

            Assert.Multiple(() =>
            {
                var valuesNotFound = result.Where(x =>
                    x.Value.StartsWith("ERROR")).ToList();

                Assert.That(valuesNotFound, Has.Exactly(2).Items);

                Assert.That(result.Any(x =>
                        x is { Description: "Get Data Asset By Id EndPoint", Value: "ERROR: test exception message" }),
                    Is.True);

                Assert.That(result.Any(x =>
                        x is { Description: "Get Esda Ownership Details Endpoint", Value: "ERROR: test exception message" }),
                    Is.True);
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var mockServiceConfigurationPresenter = new Mock<IServiceConfigurationPresenter>();

            var datasetInformationServiceConfigurationPresenter = new DataAssetInformationServiceConfigurationPresenter(
                mockServiceConfigurationPresenter.Object);

            return new TestItems(
                datasetInformationServiceConfigurationPresenter,
                mockServiceConfigurationPresenter);
        }

        private class TestItems(
            IDataAssetInformationServiceConfigurationPresenter dataAssetInformationServiceConfigurationPresenter,
            Mock<IServiceConfigurationPresenter> mockServiceConfigurationPresenter)
        {
            public IDataAssetInformationServiceConfigurationPresenter DataAssetInformationServiceConfigurationPresenter { get; } = dataAssetInformationServiceConfigurationPresenter;
            public Mock<IServiceConfigurationPresenter> MockServiceConfigurationPresenter { get; } = mockServiceConfigurationPresenter;
        }
        #endregion
    }
}
