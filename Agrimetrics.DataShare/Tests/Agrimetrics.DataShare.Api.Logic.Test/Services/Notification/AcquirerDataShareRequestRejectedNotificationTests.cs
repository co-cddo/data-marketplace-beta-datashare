using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification;

[TestFixture]
public class AcquirerDataShareRequestRejectedNotificationTests
{
    [Test]
    public void GivenNotificationConfiguration_WhenIGetTemplateId_ThenTheAcquirerDataShareRequestRejectedIdIsReturnedFromConfiguration()
    {
        var testItems = CreateTestItems();

        var testAcquirerDataShareRequestRejectedTemplateId = testItems.Fixture.Create<Guid>();

        var mockTemplateIdSet = new Mock<INotificationTemplateIdSet>();
        mockTemplateIdSet.SetupGet(x => x.AcquirerDataShareRequestRejectedId)
            .Returns(testAcquirerDataShareRequestRejectedTemplateId);

        testItems.MockNotificationConfiguration
            .Setup(x => x.TemplateIdSet)
            .Returns(mockTemplateIdSet.Object);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.TemplateId;

        Assert.That(result, Is.EqualTo(testAcquirerDataShareRequestRejectedTemplateId));
    }

    [Test]
    public void GivenAConfiguredAcquirerDataShareRequestRejectedNotification_WhenIGetPersonalisation_ThenPersonalisationItemsAreReturnedForTheRelevantProperties()
    {
        const string testAcquirerUserName = "test acquirer user user name";
        const string testEsdaName = "test esda name";
        const string testDataShareRequestRequestId = "test data share request request id";
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";
        const string testReasonsForRejection = "test reasons for rejection";

        var testItems = CreateTestItems(
            acquirerUserName: testAcquirerUserName,
            esdaName: testEsdaName,
            dataShareRequestRequestId: testDataShareRequestRequestId,
            supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress,
            reasonsForRejection: testReasonsForRejection);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.Personalisation;

        Assert.Multiple(() =>
        {
            Assert.That(result!.PersonalisationItems, Has.Exactly(5).Items);

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "acquirer-name", Value: testAcquirerUserName }),
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

            Assert.That(result.PersonalisationItems.Any(x =>
                    x is { FieldName: "reject-reason", Value: testReasonsForRejection }),
                Is.True);
        });
    }

    [Test]
    public void GivenAcquirerUserEmailAddressIsInitialised_WhenIGetRecipientEmailAddress_ThenTheInitialValueOfAcquirerUserEmailAddressIsReturned()
    {
        const string testAcquirerUserEmailAddress = "test acquirer user email address";

        var testItems = CreateTestItems(acquirerUserEmailAddress: testAcquirerUserEmailAddress);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.RecipientEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenAcquirerUserEmailAddressIsInitialised_WhenIGetAcquirerUserEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testAcquirerUserEmailAddress = "test acquirer user email address";

        var testItems = CreateTestItems(acquirerUserEmailAddress: testAcquirerUserEmailAddress);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.AcquirerUserEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenSupplierOrganisationEmailAddressIsInitialised_WhenIGetSupplierOrganisationEmailAddress_ThenTheInitialValueIsReturned()
    {
        const string testSupplierOrganisationEmailAddress = "test supplier organisation email address";

        var testItems = CreateTestItems(supplierOrganisationEmailAddress: testSupplierOrganisationEmailAddress);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.SupplierOrganisationEmailAddress;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationEmailAddress));
    }

    [Test]
    public void GivenAcquirerUserNameIsInitialised_WhenIGetAcquirerUserName_ThenTheInitialValueIsReturned()
    {
        const string testAcquirerUserName = "test acquirer user user name";

        var testItems = CreateTestItems(acquirerUserName: testAcquirerUserName);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.AcquirerUserName;

        Assert.That(result, Is.EqualTo(testAcquirerUserName));
    }

    [Test]
    public void GivenEsdaNameIsInitialised_WhenIGetEsdaName_ThenTheInitialValueIsReturned()
    {
        const string testEsdaName = "test esda name";

        var testItems = CreateTestItems(esdaName: testEsdaName);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenDataShareRequestRequestIdIsInitialised_WhenIGetDataShareRequestRequestId_ThenTheInitialValueIsReturned()
    {
        const string testDataShareRequestRequestId = "test data share request request id";

        var testItems = CreateTestItems(dataShareRequestRequestId: testDataShareRequestRequestId);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenReasonsForRejectionIsInitialised_WhenIGetReasonsForRejection_ThenTheInitialValueIsReturned()
    {
        const string testReasonsForRejection = "test reasons for rejection";

        var testItems = CreateTestItems(reasonsForRejection: testReasonsForRejection);

        var result = testItems.AcquirerDataShareRequestRejectedNotification.ReasonsForRejection;

        Assert.That(result, Is.EqualTo(testReasonsForRejection));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems(
        string? acquirerUserEmailAddress = null,
        string? supplierOrganisationEmailAddress = null,
        string? acquirerUserName = null,
        string? esdaName = null,
        string? dataShareRequestRequestId = null,
        string? reasonsForRejection = null)
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockNotificationConfiguration = Mock.Get(fixture.Freeze<INotificationConfiguration>());

        var acquirerDataShareRequestRejectedNotification = new AcquirerDataShareRequestRejectedNotification(
            mockNotificationConfiguration.Object)
        {
            AcquirerUserEmailAddress = acquirerUserEmailAddress ?? string.Empty,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress ?? string.Empty,
            AcquirerUserName = acquirerUserName ?? string.Empty,
            EsdaName = esdaName ?? string.Empty,
            DataShareRequestRequestId = dataShareRequestRequestId ?? string.Empty,
            ReasonsForRejection = reasonsForRejection ?? string.Empty
        };

        return new TestItems(
            fixture,
            acquirerDataShareRequestRejectedNotification,
            mockNotificationConfiguration);
    }

    private class TestItems(
        IFixture fixture,
        IAcquirerDataShareRequestRejectedNotification acquirerDataShareRequestRejectedNotification,
        Mock<INotificationConfiguration> mockNotificationConfiguration)
    {
        public IFixture Fixture { get; } = fixture;
        public IAcquirerDataShareRequestRejectedNotification AcquirerDataShareRequestRejectedNotification { get; } = acquirerDataShareRequestRejectedNotification;
        public Mock<INotificationConfiguration> MockNotificationConfiguration { get; } = mockNotificationConfiguration;
    }
    #endregion
}