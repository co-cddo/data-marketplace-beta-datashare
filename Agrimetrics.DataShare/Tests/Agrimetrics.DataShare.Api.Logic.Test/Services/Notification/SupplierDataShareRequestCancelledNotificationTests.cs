using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification;

[TestFixture]
public class SupplierDataShareRequestCancelledNotificationTests
{
    [Test]
    public void GivenNotificationConfiguration_WhenIGetTemplateId_ThenTheSupplierDataShareRequestCancelledIdIsReturnedFromConfiguration()
    {
        var testItems = CreateTestItems();

        var testSupplierDataShareRequestCancelledTemplateId = testItems.Fixture.Create<Guid>();

        var mockTemplateIdSet = new Mock<INotificationTemplateIdSet>();
        mockTemplateIdSet.SetupGet(x => x.SupplierDataShareRequestCancelledId)
            .Returns(testSupplierDataShareRequestCancelledTemplateId);

        testItems.MockNotificationConfiguration
            .Setup(x => x.TemplateIdSet)
            .Returns(mockTemplateIdSet.Object);

        var result = testItems.SupplierDataShareRequestCancelledNotification.TemplateId;

        Assert.That(result, Is.EqualTo(testSupplierDataShareRequestCancelledTemplateId));
    }

    [Test]
    public void GivenAConfiguredSupplierDataShareRequestCancelledNotification_WhenIGetPersonalisation_ThenPersonalisationItemsAreReturnedForTheRelevantProperties()
    {
        const string testAcquirerUserName = "test acquirer user user name";
        const string testSupplierOrganisationName = "test supplier organisation name";
        const string testEsdaName = "test esda name";
        const string testDataShareRequestRequestId = "test data share request request id";
        const string testCancellationReasons = "test cancellation reasons";

        var testItems = CreateTestItems(
            acquirerUserName: testAcquirerUserName,
            supplierOrganisationName: testSupplierOrganisationName,
            esdaName: testEsdaName,
            dataShareRequestRequestId: testDataShareRequestRequestId,
            cancellationReasons: testCancellationReasons);

        var result = testItems.SupplierDataShareRequestCancelledNotification.Personalisation;

        Assert.Multiple(() =>
        {
            Assert.That(result!.PersonalisationItems, Has.Exactly(5).Items);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "acquirer-name", Value: testAcquirerUserName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "supplier-name", Value: testSupplierOrganisationName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "resource-name", Value: testEsdaName }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "request-id", Value: testDataShareRequestRequestId }),
                Is.True);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "cancellation-reason", Value: testCancellationReasons }),
                Is.True);
        });
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetRecipientEmailAddress_ThenTheInitialValueOfSupplierOrganisationEmailAddressIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.SupplierDataShareRequestCancelledNotification.RecipientEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetSupplierOrganisationUserEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.SupplierDataShareRequestCancelledNotification.SupplierOrganisationEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationNameIsInitialised_WhenIGetSupplierOrganisationName_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationName = "test supplier organisation name";

        var testItems = CreateTestItems(supplierOrganisationName: testSupplierOrganisationName);

        var result = testItems.SupplierDataShareRequestCancelledNotification.SupplierOrganisationName;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationName));
    }

    [Test]
    public void GivenEsdaNameIsInitialised_WhenIGetEsdaName_ThenTheInitialValueIsReturned()
    {
        const string testEsdaName = "test esda name";

        var testItems = CreateTestItems(esdaName: testEsdaName);

        var result = testItems.SupplierDataShareRequestCancelledNotification.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenDataShareRequestRequestIdIsInitialised_WhenIGetDataShareRequestRequestId_ThenTheInitialValueIsReturned()
    {
        const string testDataShareRequestRequestId = "test data share request request id";

        var testItems = CreateTestItems(dataShareRequestRequestId: testDataShareRequestRequestId);

        var result = testItems.SupplierDataShareRequestCancelledNotification.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenCancellationReasonsIsInitialised_WhenIGetCancellationReasons_ThenTheInitialValueIsReturned()
    {
        const string testCancellationReasons = "test cancellation reasons";

        var testItems = CreateTestItems(cancellationReasons: testCancellationReasons);

        var result = testItems.SupplierDataShareRequestCancelledNotification.CancellationReasons;

        Assert.That(result, Is.EqualTo(testCancellationReasons));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems(
        string? acquirerUserName = null,
        string? supplierOrganisationName = null,
        string? supplierOrganisationEmailAddress = null,
        string? esdaName = null,
        string? dataShareRequestRequestId = null,
        string? cancellationReasons = null)
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockNotificationConfiguration = Mock.Get(fixture.Freeze<INotificationConfiguration>());

        var supplierDataShareRequestCancelledNotification = new SupplierDataShareRequestCancelledNotification(
            mockNotificationConfiguration.Object)
        {
            AcquirerUserName = acquirerUserName ?? string.Empty,
            SupplierOrganisationName = supplierOrganisationName ?? string.Empty,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress ?? string.Empty,
            EsdaName = esdaName ?? string.Empty,
            DataShareRequestRequestId = dataShareRequestRequestId ?? string.Empty,
            CancellationReasons = cancellationReasons ?? string.Empty
        };

        return new TestItems(
            fixture,
            supplierDataShareRequestCancelledNotification,
            mockNotificationConfiguration);
    }

    private class TestItems(
        IFixture fixture,
        ISupplierDataShareRequestCancelledNotification supplierDataShareRequestCancelledNotification,
        Mock<INotificationConfiguration> mockNotificationConfiguration)
    {
        public IFixture Fixture { get; } = fixture;
        public ISupplierDataShareRequestCancelledNotification SupplierDataShareRequestCancelledNotification { get; } = supplierDataShareRequestCancelledNotification;
        public Mock<INotificationConfiguration> MockNotificationConfiguration { get; } = mockNotificationConfiguration;
    }
    #endregion
}