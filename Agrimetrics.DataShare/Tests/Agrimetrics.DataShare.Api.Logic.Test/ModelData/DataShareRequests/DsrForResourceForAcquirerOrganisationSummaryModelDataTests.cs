using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestForResourceForAcquirerOrganisationSummaryModelDataTests
{
    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetRequestId_ThenRequestIdIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testRequestId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestId = testRequestId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerUserId = testAcquirerUserId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetAcquirerDomainId_ThenAcquirerDomainIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerDomainId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerDomainId = testAcquirerDomainId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerDomainId;

        Assert.That(result, Is.EqualTo(testAcquirerDomainId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetSupplierDomainId_ThenSupplierDomainIdIsSet(
        [Values(-1, 0, 999)] int testSupplierDomainId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierDomainId = testSupplierDomainId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierDomainId;

        Assert.That(result, Is.EqualTo(testSupplierDomainId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierOrganisationId = testSupplierOrganisationId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetEsdaId_ThenEsdaIdIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testEsdaId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_EsdaId = testEsdaId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_EsdaId;

        Assert.That(result, Is.EqualTo(testEsdaId));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetRequestRequestId_ThenRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestRequestId)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestRequestId = testRequestRequestId;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestRequestId;

        Assert.That(result, Is.EqualTo(testRequestRequestId));
    }

    [Theory]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestStatus = testRequestStatus;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetWhenCreatedLocal_ThenWhenCreatedLocalIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testWhenCreatedLocal = new DateTime(2025, 12, 25, 14, 45, 59);

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenCreatedLocal = testWhenCreatedLocal;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenCreatedLocal;

        Assert.That(result, Is.EqualTo(testWhenCreatedLocal));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetANullWhenSubmittedLocal_ThenWhenSubmittedLocalIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testWhenSubmittedLocal = (DateTime?) null;

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenSubmittedLocal = testWhenSubmittedLocal;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenSubmittedLocal;

        Assert.That(result, Is.EqualTo(testWhenSubmittedLocal));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetWhenSubmittedLocal_ThenWhenSubmittedLocalIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testWhenSubmittedLocal = new DateTime(2025, 12, 25, 14, 45, 59);

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenSubmittedLocal = testWhenSubmittedLocal;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.WhenSubmittedLocal;

        Assert.That(result, Is.EqualTo(testWhenSubmittedLocal));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryModelData_WhenISetOwnerContactDetails_ThenOwnerContactDetailsIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryModelData();

        var testOwnerContactDetails = new DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_OwnerContactDetails = testOwnerContactDetails;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryModelData.DataShareRequestForResourceForAcquirerOrganisationSummary_OwnerContactDetails;

        Assert.That(result, Is.EqualTo(testOwnerContactDetails));
    }
}
