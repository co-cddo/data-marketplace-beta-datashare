using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification;

[TestFixture]
public class AcquirerDataShareRequestAcceptedNotificationTests
{
    [Test]
    public void GivenNotificationConfiguration_WhenIGetTemplateId_ThenTheAcquirerDataShareRequestAcceptedIdIsReturnedFromConfiguration()
    {
        var testItems = CreateTestItems();

        var testAcquirerDataShareRequestAcceptedTemplateId = testItems.Fixture.Create<Guid>();

        var mockTemplateIdSet = new Mock<INotificationTemplateIdSet>();
        mockTemplateIdSet.SetupGet(x => x.AcquirerDataShareRequestAcceptedId)
            .Returns(testAcquirerDataShareRequestAcceptedTemplateId);

        testItems.MockNotificationConfiguration
            .Setup(x => x.TemplateIdSet)
            .Returns(mockTemplateIdSet.Object);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.TemplateId;

        Assert.That(result, Is.EqualTo(testAcquirerDataShareRequestAcceptedTemplateId));
    }

    [Test]
    public void GivenAConfiguredAcquirerDataShareRequestAcceptedNotification_WhenIGetPersonalisation_ThenPersonalisationItemsAreReturnedForTheRelevantProperties()
    {
        const string testAcquirerUserName = "test acquirer user user name";
        const string testSupplierOrganisationName = "test supplier organisation name";
        const string testEsdaName = "test esda name";
        const string testDataShareRequestRequestId = "test data share request request id";
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(
            acquirerUserName: testAcquirerUserName,
            supplierOrganisationName: testSupplierOrganisationName,
            esdaName: testEsdaName,
            dataShareRequestRequestId: testDataShareRequestRequestId,
            supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.Personalisation;

        Assert.Multiple(() =>
        {
            Assert.That(result!.PersonalisationItems, Has.Exactly(5).Items);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is {FieldName: "acquirer-name", Value: testAcquirerUserName}),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is {FieldName: "supplier-name", Value: testSupplierOrganisationName}),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "dataset-title", Value: testEsdaName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "request-ID", Value: testDataShareRequestRequestId }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "supplier-email", Value: testSupplierOrganisationEmailAddress }),
                Is.True);
        });
    }

    [Test]
    public void GivenAcquirerUserEmailAddressIsInitialised_WhenIGetRecipientEmailAddress_ThenTheInitialValueOfAcquirerUserEmailAddressIsReturned()
    {
        const string testAcquirerUserEmailAddress = "test acquirer user email address";

        var testItems = CreateTestItems(acquirerUserEmailAddress: testAcquirerUserEmailAddress);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.RecipientEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenAcquirerUserEmailAddressIsInitialised_WhenIGetAcquirerUserEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testAcquirerUserEmailAddress = "test acquirer user email address";

        var testItems = CreateTestItems(acquirerUserEmailAddress: testAcquirerUserEmailAddress);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.AcquirerUserEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetSupplierOrganisationEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.SupplierOrganisationEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationNameIsInitialised_WhenIGetSupplierOrganisationName_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationName = "test supplier organisation name";

        var testItems = CreateTestItems(supplierOrganisationName: testSupplierOrganisationName);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.SupplierOrganisationName;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationName));
    }

    [Test]
    public void GivenAcquirerUserNameIsInitialised_WhenIGetAcquirerUserName_ThenTheInitialValueIsReturned()
    {
        const string testAcquirerUserName = "test acquirer user user name";

        var testItems = CreateTestItems(acquirerUserName: testAcquirerUserName);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.AcquirerUserName;

        Assert.That(result, Is.EqualTo(testAcquirerUserName));
    }
    
    [Test]
    public void GivenEsdaNameIsInitialised_WhenIGetEsdaName_ThenTheInitialValueIsReturned()
    {
        const string testEsdaName = "test esda name";

        var testItems = CreateTestItems(esdaName: testEsdaName);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }
    
    [Test]
    public void GivenDataShareRequestRequestIdIsInitialised_WhenIGetDataShareRequestRequestId_ThenTheInitialValueIsReturned()
    {
        const string testDataShareRequestRequestId = "test data share request request id";

        var testItems = CreateTestItems(dataShareRequestRequestId: testDataShareRequestRequestId);

        var result = testItems.AcquirerDataShareRequestAcceptedNotification.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }
    
    #region Test Item Creation
    private static TestItems CreateTestItems(
        string? acquirerUserEmailAddress = null,
        string? supplierOrganisationEmailAddress = null,
        string? supplierOrganisationName = null,
        string? acquirerUserName = null,
        string? esdaName = null,
        string? dataShareRequestRequestId = null)
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockNotificationConfiguration = Mock.Get(fixture.Freeze<INotificationConfiguration>());

        var acquirerDataShareRequestAcceptedNotification = new AcquirerDataShareRequestAcceptedNotification(
            mockNotificationConfiguration.Object)
        {
            AcquirerUserEmailAddress = acquirerUserEmailAddress ?? string.Empty,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress ?? string.Empty,
            SupplierOrganisationName = supplierOrganisationName ?? string.Empty,
            AcquirerUserName = acquirerUserName ?? string.Empty,
            EsdaName = esdaName ?? string.Empty,
            DataShareRequestRequestId = dataShareRequestRequestId ?? string.Empty
        };

        return new TestItems(
            fixture,
            acquirerDataShareRequestAcceptedNotification,
            mockNotificationConfiguration);
    }

    private class TestItems(
        IFixture fixture,
        IAcquirerDataShareRequestAcceptedNotification acquirerDataShareRequestAcceptedNotification,
        Mock<INotificationConfiguration> mockNotificationConfiguration)
    {
        public IFixture Fixture { get; } = fixture;
        public IAcquirerDataShareRequestAcceptedNotification AcquirerDataShareRequestAcceptedNotification { get; } = acquirerDataShareRequestAcceptedNotification;
        public Mock<INotificationConfiguration> MockNotificationConfiguration { get; } = mockNotificationConfiguration;
    }
    #endregion
}