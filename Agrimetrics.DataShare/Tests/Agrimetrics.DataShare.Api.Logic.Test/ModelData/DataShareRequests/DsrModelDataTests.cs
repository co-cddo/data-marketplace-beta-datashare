using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestModelDataTests
{
    [Test]
    public void GivenADataShareRequestModelData_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        var testId = new Guid("84155261-2BF2-4C18-A5E9-C973A1AA0F10");

        testDataShareRequestModelData.DataShareRequest_Id = testId;

        var result = testDataShareRequestModelData.DataShareRequest_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_RequestId = testRequestId;

        var result = testDataShareRequestModelData.DataShareRequest_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_AcquirerUserId = testAcquirerUserId;

        var result = testDataShareRequestModelData.DataShareRequest_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetAcquirerDomainId_ThenAcquirerDomainIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerDomainId)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_AcquirerDomainId = testAcquirerDomainId;

        var result = testDataShareRequestModelData.DataShareRequest_AcquirerDomainId;

        Assert.That(result, Is.EqualTo(testAcquirerDomainId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testDataShareRequestModelData.DataShareRequest_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_SupplierOrganisationId = testSupplierOrganisationId;

        var result = testDataShareRequestModelData.DataShareRequest_SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetEsdaId_ThenEsdaIdIsSet()
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        var testEsdaId = new Guid("84155261-2BF2-4C18-A5E9-C973A1AA0F10");

        testDataShareRequestModelData.DataShareRequest_EsdaId = testEsdaId;

        var result = testDataShareRequestModelData.DataShareRequest_EsdaId;

        Assert.That(result, Is.EqualTo(testEsdaId));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_EsdaName = testEsdaName;

        var result = testDataShareRequestModelData.DataShareRequest_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Theory]
    public void GivenADataShareRequestModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_RequestStatus = testRequestStatus;

        var result = testDataShareRequestModelData.DataShareRequest_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Theory]
    public void GivenADataShareRequestModelData_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        testDataShareRequestModelData.DataShareRequest_QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testDataShareRequestModelData.DataShareRequest_QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenADataShareRequestModelData_WhenISetQuestionSetId_ThenQuestionSetIdIsSet()
    {
        var testDataShareRequestModelData = new DataShareRequestModelData();

        var testQuestionSetId = new Guid("84155261-2BF2-4C18-A5E9-C973A1AA0F10");

        testDataShareRequestModelData.DataShareRequest_QuestionSetId = testQuestionSetId;

        var result = testDataShareRequestModelData.DataShareRequest_QuestionSetId;

        Assert.That(result, Is.EqualTo(testQuestionSetId));
    }
}