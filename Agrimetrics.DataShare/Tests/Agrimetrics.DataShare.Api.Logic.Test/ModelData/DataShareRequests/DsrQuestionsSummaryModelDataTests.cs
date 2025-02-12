using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestQuestionsSummaryModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        var testId = new Guid("301112FD-6559-462B-A325-41264883B57D");

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_Id = testId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_RequestId = testRequestId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_EsdaName = testEsdaName;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerUserId = testAcquirerUserId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetAcquirerDomainId_ThenAcquirerDomainIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerDomainId)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerDomainId = testAcquirerDomainId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerDomainId;

        Assert.That(result, Is.EqualTo(testAcquirerDomainId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationId = testSupplierOrganisationId;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetQuestionSetSummary_ThenQuestionSetSummaryIsSet()
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        var testQuestionSetSummary = new QuestionSetSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary = testQuestionSetSummary;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary;

        Assert.That(result, Is.EqualTo(testQuestionSetSummary));
    }

    [Theory]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus = testDataShareRequestStatus;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }

    [Theory]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetSupplierOrganisationName_ThenSupplierOrganisationNameIsSet(
        [Values(null, "", "  ", "abc")] string? testSupplierOrganisationName)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationName = testSupplierOrganisationName;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationName;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationName));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetSubmissionResponseFromSupplier_ThenSubmissionResponseFromSupplierIsSet(
        [Values(null, "", "  ", "abc")] string? testSubmissionResponseFromSupplier)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SubmissionResponseFromSupplier = testSubmissionResponseFromSupplier;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SubmissionResponseFromSupplier;

        Assert.That(result, Is.EqualTo(testSubmissionResponseFromSupplier));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenISetCancellationReasonsFromAcquirer_ThenCancellationReasonsFromAcquirerIsSet(
        [Values(null, "", "  ", "abc")] string? testCancellationReasonsFromAcquirer)
    {
        var testDataShareRequestQuestionsSummaryModelData = new DataShareRequestQuestionsSummaryModelData();

        testDataShareRequestQuestionsSummaryModelData.DataShareRequest_CancellationReasonsFromAcquirer = testCancellationReasonsFromAcquirer;

        var result = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_CancellationReasonsFromAcquirer;

        Assert.That(result, Is.EqualTo(testCancellationReasonsFromAcquirer));
    }
}