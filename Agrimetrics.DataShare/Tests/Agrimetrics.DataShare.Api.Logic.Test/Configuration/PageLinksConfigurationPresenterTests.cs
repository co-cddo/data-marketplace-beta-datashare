using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Configuration;

[TestFixture]
public class PageLinksConfigurationPresenterTests
{
    #region GetDataMarketPlaceSignInAddress() Tests
    [Test]
    public void GivenAPageLinksConfigurationPresenter_WhenIGetDataMarketPlaceSignInAddress_ThenTheRelevantValueIsReadFromServiceConfiguration()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "PageLinks",
                "data_marketplace_sign_in_address"))
            .Returns(() => "test data marketplace sign in address");

        var result = testItems.PageLinksConfigurationPresenter.GetDataMarketPlaceSignInAddress();

        Assert.That(result, Is.EqualTo("test data marketplace sign in address"));
    }
    #endregion

    #region GetAllSettings()
    [Test]
    public void GivenAPageLinksConfigurationPresenter_WhenIGetAllSettingValues_ThenADescriptionOfAllSettingsIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "PageLinks", "data_marketplace_sign_in_address"))
            .Returns(() => "test data marketplace sign in address");

        var result = testItems.PageLinksConfigurationPresenter.GetAllSettings().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.One.Items);

            Assert.That(result.Any(x =>
                    x is { Description: "Get Data Market Place Sign In Address", Value: "test data marketplace sign in address" }),
                Is.True);
        });

    }

    [Test]
    public void GivenGettingAConfigurationValueWithThrowAnException_WhenIGetAllSettingValues_ThenAnEmptyStringIsReturnedForThatValue()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "PageLinks", "data_marketplace_sign_in_address"))
            .Throws(new Exception("error 1"));

        var result = testItems.PageLinksConfigurationPresenter.GetAllSettings().ToList();

        Assert.Multiple(() =>
        {
            var valuesNotFound = result.Where(x =>
                x.Value.StartsWith("ERROR")).ToList();

            Assert.That(valuesNotFound, Has.One.Items);

            Assert.That(result.FirstOrDefault(x =>
                x is { Description: "Get Data Market Place Sign In Address", Value: "ERROR: error 1" }), Is.Not.Null);
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var mockServiceConfigurationPresenter = new Mock<IServiceConfigurationPresenter>();

        var pageLinksConfigurationPresenter = new PageLinksConfigurationPresenter(
            mockServiceConfigurationPresenter.Object);

        return new TestItems(
            pageLinksConfigurationPresenter,
            mockServiceConfigurationPresenter);
    }

    private class TestItems(
        IPageLinksConfigurationPresenter pageLinksConfigurationPresenter,
        Mock<IServiceConfigurationPresenter> mockServiceConfigurationPresenter)
    {
        public IPageLinksConfigurationPresenter PageLinksConfigurationPresenter { get; } = pageLinksConfigurationPresenter;
        public Mock<IServiceConfigurationPresenter> MockServiceConfigurationPresenter { get; } = mockServiceConfigurationPresenter;
    }
    #endregion
}