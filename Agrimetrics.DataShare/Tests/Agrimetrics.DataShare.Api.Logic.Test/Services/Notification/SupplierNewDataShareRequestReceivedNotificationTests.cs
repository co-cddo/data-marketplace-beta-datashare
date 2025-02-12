using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification;

[TestFixture]
public class SupplierNewDataShareRequestReceivedNotificationTests
{
    [Test]
    public void GivenNotificationConfiguration_WhenIGetTemplateId_ThenTheSupplierNewDataShareRequestReceivedIdIsReturnedFromConfiguration()
    {
        var testItems = CreateTestItems();

        var testSupplierNewDataShareRequestReceivedTemplateId = testItems.Fixture.Create<Guid>();

        var mockTemplateIdSet = new Mock<INotificationTemplateIdSet>();
        mockTemplateIdSet.SetupGet(x => x.SupplierNewDataShareRequestReceivedId)
            .Returns(testSupplierNewDataShareRequestReceivedTemplateId);

        testItems.MockNotificationConfiguration
            .Setup(x => x.TemplateIdSet)
            .Returns(mockTemplateIdSet.Object);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.TemplateId;

        Assert.That(result, Is.EqualTo(testSupplierNewDataShareRequestReceivedTemplateId));
    }

    [Test]
    public void GivenAConfiguredSupplierNewDataShareRequestReceivedNotification_WhenIGetPersonalisation_ThenPersonalisationItemsAreReturnedForTheRelevantProperties()
    {
        const string testAcquirerOrganisationName = "test acquirer organisation name";
        const string testSupplierOrganisationName = "test supplier organisation name";
        const string testEsdaName = "test esda name";
        const string testDataMarketPlaceSignInAddress = "test data market place sign in address";

        var testItems = CreateTestItems(
            acquirerOrganisationName: testAcquirerOrganisationName,
            supplierOrganisationName: testSupplierOrganisationName,
            esdaName: testEsdaName,
            dataMarketPlaceSignInAddress: testDataMarketPlaceSignInAddress);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.Personalisation;

        Assert.Multiple(() =>
        {
            Assert.That(result!.PersonalisationItems, Has.Exactly(4).Items);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "acquirer-organisation", Value: testAcquirerOrganisationName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "supplier-name", Value: testSupplierOrganisationName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "resource-name", Value: testEsdaName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "sign-in", Value: $"[sign in]({testDataMarketPlaceSignInAddress})" }),
                Is.True);
        });
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetRecipientEmailAddress_ThenTheInitialValueOfSupplierOrganisationEmailAddressIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.RecipientEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetSupplierOrganisationUserEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.SupplierOrganisationEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationNameIsInitialised_WhenIGetSupplierOrganisationName_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationName = "test supplier organisation name";

        var testItems = CreateTestItems(supplierOrganisationName: testSupplierOrganisationName);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.SupplierOrganisationName;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationName));
    }

    [Test]
    public void GivenAcquirerOrganisationNameIsInitialised_WhenIGetAcquirerOrganisationName_ThenTheInitialValueIsReturned()
    {
        const string testAcquirerOrganisationName = "test acquirer organisation name";

        var testItems = CreateTestItems(acquirerOrganisationName: testAcquirerOrganisationName);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenEsdaNameIsInitialised_WhenIGetEsdaName_ThenTheInitialValueIsReturned()
    {
        const string testEsdaName = "test esda name";

        var testItems = CreateTestItems(esdaName: testEsdaName);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenDataMarketPlaceSignInAddressIsInitialised_WhenIGetDataMarketPlaceSignInAddress_ThenTheInitialValueIsReturned()
    {
        const string testDataMarketPlaceSignInAddress = "test data market place sign in address";

        var testItems = CreateTestItems(dataMarketPlaceSignInAddress: testDataMarketPlaceSignInAddress);

        var result = testItems.SupplierNewDataShareRequestReceivedNotification.DataMarketPlaceSignInAddress;

        Assert.That(result, Is.EqualTo(testDataMarketPlaceSignInAddress));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems(
        string? supplierOrganisationEmailAddress = null,
        string? supplierOrganisationName = null,
        string? acquirerOrganisationName = null,
        string? esdaName = null,
        string? dataMarketPlaceSignInAddress = null)
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockNotificationConfiguration = Mock.Get(fixture.Freeze<INotificationConfiguration>());

        var supplierDataShareRequestCancelledNotification = new SupplierNewDataShareRequestReceivedNotification(
            mockNotificationConfiguration.Object)
        {
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress ?? string.Empty,
            SupplierOrganisationName = supplierOrganisationName ?? string.Empty,
            AcquirerOrganisationName = acquirerOrganisationName ?? string.Empty,
            EsdaName = esdaName ?? string.Empty,
            DataMarketPlaceSignInAddress = dataMarketPlaceSignInAddress ?? string.Empty
        };

        return new TestItems(
            fixture,
            supplierDataShareRequestCancelledNotification,
            mockNotificationConfiguration);
    }

    private class TestItems(
        IFixture fixture,
        ISupplierNewDataShareRequestReceivedNotification supplierDataShareRequestCancelledNotification,
        Mock<INotificationConfiguration> mockNotificationConfiguration)
    {
        public IFixture Fixture { get; } = fixture;
        public ISupplierNewDataShareRequestReceivedNotification SupplierNewDataShareRequestReceivedNotification { get; } = supplierDataShareRequestCancelledNotification;
        public Mock<INotificationConfiguration> MockNotificationConfiguration { get; } = mockNotificationConfiguration;
    }
    #endregion
}