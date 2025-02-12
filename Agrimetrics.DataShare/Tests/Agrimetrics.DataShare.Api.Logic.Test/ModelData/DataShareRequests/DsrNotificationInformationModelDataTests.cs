using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestNotificationInformationModelDataTests
{
    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        testDataShareRequestNotificationInformationModelData.SupplierOrganisationId = testSupplierOrganisationId;

        var result = testDataShareRequestNotificationInformationModelData.SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetSupplierDomainId_ThenSupplierDomainIdIsSet(
        [Values(-1, 0, 999)] int testSupplierDomainId)
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        testDataShareRequestNotificationInformationModelData.SupplierDomainId = testSupplierDomainId;

        var result = testDataShareRequestNotificationInformationModelData.SupplierDomainId;

        Assert.That(result, Is.EqualTo(testSupplierDomainId));
    }

    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        testDataShareRequestNotificationInformationModelData.AcquirerUserId = testAcquirerUserId;

        var result = testDataShareRequestNotificationInformationModelData.AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        testDataShareRequestNotificationInformationModelData.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testDataShareRequestNotificationInformationModelData.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetEsdaId_ThenEsdaIdIsSet()
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        var testEsdaId = new Guid("ED4A1BEA-4824-4B50-97CD-435A8EB5E291");

        testDataShareRequestNotificationInformationModelData.EsdaId = testEsdaId;

        var result = testDataShareRequestNotificationInformationModelData.EsdaId;

        Assert.That(result, Is.EqualTo(testEsdaId));
    }

    [Test]
    public void GivenADataShareRequestNotificationInformationModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestNotificationInformationModelData = new DataShareRequestNotificationInformationModelData();

        testDataShareRequestNotificationInformationModelData.EsdaName = testEsdaName;

        var result = testDataShareRequestNotificationInformationModelData.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }
}