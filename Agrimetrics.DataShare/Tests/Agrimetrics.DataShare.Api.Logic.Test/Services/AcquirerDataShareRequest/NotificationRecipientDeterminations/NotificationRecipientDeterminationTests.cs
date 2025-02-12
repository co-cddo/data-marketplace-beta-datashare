using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;

[TestFixture]
public class DataShareRequestNotificationRecipientDeterminationTests
{
    #region DetermineDataShareRequestNotificationRecipientAsync() Tests
    #region Esda Configured with Domain Dsr Notification Address Recipient
    [Test]
    public void GivenAnEsdaThatIsConfiguredWithADomainDsrNotificationRecipientAndNoMatchingDomainInformationIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;
        const int testEsdaDomainId = 456;

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationId: testEsdaOrganisationId,
                domains: [CreateTestDomainInformation(domainId: testEsdaDomainId)]));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId,
            supplierDomainId: testEsdaDomainId + 1000);

        var resultException = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData));

        Assert.That(resultException!.Message, Is.EqualTo("Unable to determine Data Share Request Notification Recipient for ESDA with mismatching supplier domain.  '1456'"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithADomainDsrNotificationRecipientAndNotificationsAreDisabledForTheDomain_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientEmailAddressIsTheCddoAdministratorAddress()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;
        const int testEsdaDomainId = 456;
        const string testCddoAdminEmailAddress = "cddo admin email address";

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationId: testEsdaOrganisationId,
                domains: [CreateTestDomainInformation(domainId: testEsdaDomainId)]));
        
        testItems.MockNotificationsConfigurationPresenter
            .Setup(x => x.GetDataShareRequestNotificationCddoAdminEmailAddress())
            .Returns(testCddoAdminEmailAddress);

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId,
            supplierDomainId: testEsdaDomainId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.EmailAddress, Is.EqualTo(testCddoAdminEmailAddress));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithADomainDsrNotificationRecipientAndNotificationsAreDisabledForTheDomain_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsTheCddoAdministratorName()
    {
        var testItems = CreateTestItems();


        const int testEsdaOrganisationId = 123;
        const int testEsdaDomainId = 456;
        const string testCddoAdminName = "cddo admin name";


        testItems.MockNotificationsConfigurationPresenter
            .Setup(x => x.GetDataShareRequestNotificationCddoAdminName())
            .Returns(testCddoAdminName);

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationId: testEsdaOrganisationId,
                domains: [CreateTestDomainInformation(domainId: testEsdaDomainId, dataShareRequestMailboxAddress: null)]));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId,
            supplierDomainId: testEsdaDomainId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo(testCddoAdminName));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithADomainDsrNotificationRecipientAndNotificationsAreEnabledForTheDomain_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientEmailAddressIsTheDomainDsrNotificationAddress()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;
        const int testEsdaDomainId = 456;

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationId: testEsdaOrganisationId,
                domains: [CreateTestDomainInformation(domainId: testEsdaDomainId, dataShareRequestMailboxAddress: "domain dsr email address")]));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId,
            supplierDomainId: testEsdaDomainId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.EmailAddress, Is.EqualTo("domain dsr email address"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithADomainDsrNotificationRecipientAndNotificationsAreEnabledForTheDomain_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsThePrettifiedNameOfTheOrganisationThatOwnsTheEsda()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;
        const int testEsdaDomainId = 456;

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationName: "test-organisation-name",
                organisationId: testEsdaOrganisationId,
                domains: [CreateTestDomainInformation(domainId: testEsdaDomainId, dataShareRequestMailboxAddress: "something non-empty")]));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId,
            supplierDomainId: testEsdaDomainId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo("Test Organisation Name"));
    }
    #endregion

    #region Esda Configured with Esda Contact Point Dsr Notification Address Recipient
    [Test]
    public void GivenAnEsdaThatIsConfiguredWithAnEsdaContactPointDsrNotificationRecipientAndNoContactPointEmailAddressIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenAnInvalidOperationExceptionIsThrown(
        [Values(null, "", "  ")] string? testContactPointEmailAddress)
    {
        var testItems = CreateTestItems();

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaContactPointEmailAddress,
            contactPointEmailAddress: testContactPointEmailAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId);

        var resultException = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData));

        Assert.That(resultException!.Message, Is.EqualTo($"Unable to determine Data Share Request Notification Recipient for ESDA configured with empty Contact Point address.  '{testEsdaId}'"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithAnEsdaContactPointDsrNotificationRecipientAndAContactPointEmailAddressIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientEmailAddressIsTheContactPointEmailAddress()
    {
        var testItems = CreateTestItems();

        const string testContactPointEmailAddress = "test contact point address";

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaContactPointEmailAddress,
            contactPointEmailAddress: testContactPointEmailAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.EmailAddress, Is.EqualTo(testContactPointEmailAddress));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithAnEsdaContactPointDsrNotificationRecipientAndAContactPointNameIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsTheContactPointName()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationName: "test-organisation-name",
                organisationId:testEsdaOrganisationId));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaContactPointEmailAddress,
            contactPointEmailAddress: "something non-empty",
            contactPointName: "test contact point name");

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo("test contact point name"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithAnEsdaContactPointDsrNotificationRecipientAndNoContactPointNameIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsThePrettifiedNameOfTheOrganisationThatOwnsTheEsda(
        [Values(null, "", "   ")] string? testContactPointName)
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;
        const string testContactPointEmailAddress = "test contact point address";

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationName: "test-organisation-name",
                organisationId: testEsdaOrganisationId));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaContactPointEmailAddress,
            contactPointEmailAddress: testContactPointEmailAddress,
            contactPointName: testContactPointName);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo("Test Organisation Name"));
    }
    #endregion

    #region Esda Configured with Esda Custom Dsr Notification Address Recipient
    [Test]
    public void GivenAnEsdaThatIsConfiguredWithAnEsdaCustomDsrNotificationRecipientAndNoCustomEmailAddressIsAvailable_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenAnInvalidOperationExceptionIsThrown(
        [Values(null, "", "  ")] string? testContactPointEmailAddress)
    {
        var testItems = CreateTestItems();

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaCustomDsrNotificationAddress,
            contactPointEmailAddress: testContactPointEmailAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId);

        var resultException = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData));

        Assert.That(resultException!.Message, Is.EqualTo($"Unable to determine Data Share Request Notification Recipient for ESDA configured with empty Custom Recipient.  '{testEsdaId}'"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithAnEsdaCustomDsrNotificationRecipient_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientEmailAddressIsTheCustomEmailAddressOnTheEsda()
    {
        var testItems = CreateTestItems();

        const string testCustomEmailAddress = "test custom address";

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaCustomDsrNotificationAddress,
            customDsrNotificationAddress: testCustomEmailAddress);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.EmailAddress, Is.EqualTo(testCustomEmailAddress));
    }

    [Test]
    public async Task GivenAnEsdaThatIsConfiguredWithAnEsdaCustomDsrNotificationRecipient_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsThePrettifiedNameOfTheOrganisationThatOwnsTheEsda()
    {
        var testItems = CreateTestItems();

        const int testEsdaOrganisationId = 123;

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(testEsdaOrganisationId))
            .ReturnsAsync(() => CreateTestOrganisationInformation(
                organisationName: "test-organisation-name",
                organisationId: testEsdaOrganisationId));

        var testEsdaId = testItems.Fixture.Create<Guid>();

        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: DataShareRequestNotificationRecipientType.EsdaCustomDsrNotificationAddress,
            customDsrNotificationAddress: "something non-empty");

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData(
            esdaId: testEsdaId,
            supplierOrganisationId: testEsdaOrganisationId);

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo("Test Organisation Name"));
    }
    #endregion

    #region Esda Not Configured with Notification Address Recipient
    [Test]
    public async Task GivenAnEsdaThatIsNotConfiguredWithADsrNotificationRecipient_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientEmailAddressIsTheCddoAdministratorAddress()
    {
        var testItems = CreateTestItems();

        testItems.MockNotificationsConfigurationPresenter
            .Setup(x => x.GetDataShareRequestNotificationCddoAdminEmailAddress())
            .Returns("cddo admin email address");

        var testEsdaId = testItems.Fixture.Create<Guid>();
        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: null);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData();

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.EmailAddress, Is.EqualTo("cddo admin email address"));
    }

    [Test]
    public async Task GivenAnEsdaThatIsNotConfiguredWithADsrNotificationRecipient_WhenIDetermineDataShareRequestNotificationRecipientAsync_ThenTheDeterminedRecipientNameIsTheNameOfTheOrganisationThatOwnsTheEsda()
    {
        var testItems = CreateTestItems();

        testItems.MockNotificationsConfigurationPresenter
            .Setup(x => x.GetDataShareRequestNotificationCddoAdminName())
            .Returns("cddo admin name");

        var testEsdaId = testItems.Fixture.Create<Guid>();
        var testEsdaDetails = CreateTestEsdaDetails(
            esdaId: testEsdaId,
            dataShareRequestNotificationRecipientType: null);

        testItems.MockEsdaInformationPresenter
            .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
            .ReturnsAsync(() => testEsdaDetails);

        var dataShareRequestNotificationInformationModelData = CreateTestDataShareRequestNotificationInformationModelData();

        var result = await testItems.DataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformationModelData);

        Assert.That(result.RecipientName, Is.EqualTo("cddo admin name"));
    }
    #endregion
    #endregion

    #region Test Data Creation
    private static DataShareRequestNotificationInformationModelData CreateTestDataShareRequestNotificationInformationModelData(
        Guid? esdaId = null,
        int? supplierOrganisationId = null,
        int? supplierDomainId = null)
    {
        return new DataShareRequestNotificationInformationModelData
        {
            EsdaId = esdaId ?? Guid.Empty,
            SupplierOrganisationId = supplierOrganisationId ?? 0,
            SupplierDomainId = supplierDomainId ?? 0
        };
    }

    private static IEsdaDetails CreateTestEsdaDetails(
        Guid? esdaId = null,
        DataShareRequestNotificationRecipientType? dataShareRequestNotificationRecipientType = null,
        string? customDsrNotificationAddress = null, 
        string? contactPointEmailAddress = null,
        string? contactPointName = null)
    {
        var mockEsdaDetails = new Mock<IEsdaDetails>();

        mockEsdaDetails.SetupGet(x => x.Id).Returns(esdaId ?? Guid.Empty);

        mockEsdaDetails.SetupGet(x => x.DataShareRequestNotificationRecipientType).Returns(dataShareRequestNotificationRecipientType);
        mockEsdaDetails.SetupGet(x => x.CustomDsrNotificationAddress).Returns(customDsrNotificationAddress);

        mockEsdaDetails.SetupGet(x => x.ContactPointEmailAddress).Returns(contactPointEmailAddress);
        mockEsdaDetails.SetupGet(x => x.ContactPointName).Returns(contactPointName);

        return mockEsdaDetails.Object;
    }

    private static IDomainInformation CreateTestDomainInformation(
        int? domainId = null,
        string? domainName = null,
        string? dataShareRequestMailboxAddress = null)
    {
        var mockDomainInformation = new Mock<IDomainInformation>();

        mockDomainInformation.SetupGet(x => x.DomainId).Returns(domainId ?? 0);
        mockDomainInformation.SetupGet(x => x.DomainName).Returns(domainName ?? string.Empty);
        mockDomainInformation.SetupGet(x => x.DataShareRequestMailboxAddress).Returns(dataShareRequestMailboxAddress);

        return mockDomainInformation.Object;
    }

    private static IOrganisationInformation CreateTestOrganisationInformation(
        int? organisationId = null,
        string? organisationName = null,
        IEnumerable<IDomainInformation>? domains = null)
    {
        var mockOrganisationInformation = new Mock<IOrganisationInformation>();

        mockOrganisationInformation.SetupGet(x => x.OrganisationId).Returns(organisationId ?? 0);
        mockOrganisationInformation.SetupGet(x => x.OrganisationName).Returns(organisationName ?? string.Empty);
        mockOrganisationInformation.SetupGet(x => x.Domains).Returns(domains?.ToList() ?? []);

        return mockOrganisationInformation.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockEsdaInformationPresenter = Mock.Get(fixture.Create<IEsdaInformationPresenter>());
        var mockUserProfilePresenter = Mock.Get(fixture.Create<IUserProfilePresenter>());
        var mockNotificationsConfigurationPresenter = Mock.Get(fixture.Create<INotificationsConfigurationPresenter>());

        ConfigureHappyPathTesting();

        var dataShareRequestNotificationRecipientDetermination = new DataShareRequestNotificationRecipientDetermination(
            mockEsdaInformationPresenter.Object,
            mockUserProfilePresenter.Object,
            mockNotificationsConfigurationPresenter.Object);
            
        return new TestItems(
            fixture,
            dataShareRequestNotificationRecipientDetermination,
            mockEsdaInformationPresenter,
            mockUserProfilePresenter,
            mockNotificationsConfigurationPresenter);

        void ConfigureHappyPathTesting()
        {
            mockEsdaInformationPresenter
                .Setup(x => x.GetEsdaDetailsByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => CreateTestEsdaDetails());

            mockUserProfilePresenter
                .Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => CreateTestOrganisationInformation());
        }
    }

    private class TestItems(
        IFixture fixture,
        IDataShareRequestNotificationRecipientDetermination dataShareRequestNotificationRecipientDetermination,
        Mock<IEsdaInformationPresenter> mockEsdaInformationPresenter,
        Mock<IUserProfilePresenter> mockUserProfilePresenter,
        Mock<INotificationsConfigurationPresenter> mockNotificationsConfigurationPresenter)
    {
        public IFixture Fixture { get; } = fixture;
        public IDataShareRequestNotificationRecipientDetermination DataShareRequestNotificationRecipientDetermination { get; } = dataShareRequestNotificationRecipientDetermination;
        public Mock<IEsdaInformationPresenter> MockEsdaInformationPresenter { get; } = mockEsdaInformationPresenter;
        public Mock<IUserProfilePresenter> MockUserProfilePresenter { get; } = mockUserProfilePresenter;
        public Mock<INotificationsConfigurationPresenter> MockNotificationsConfigurationPresenter { get; } = mockNotificationsConfigurationPresenter;
    }

    #endregion
}