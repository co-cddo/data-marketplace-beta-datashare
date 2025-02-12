using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        var testDataShareRequestId = new Guid("67F19CE4-7323-4BBB-92A9-44F798128BF6");

        testDataShareRequestAnswersSummary.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestAnswersSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.RequestId = testRequestId;

        var result = testDataShareRequestAnswersSummary.RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.EsdaName = testEsdaName;

        var result = testDataShareRequestAnswersSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummary_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatus testDataShareRequestStatus)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.DataShareRequestStatus = testDataShareRequestStatus;

        var result = testDataShareRequestAnswersSummary.DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummary_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testDataShareRequestAnswersSummary.QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetAnEmptySetOfSummarySections_ThenSummarySectionsIsSet()
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        var testSummarySections = new List<DataShareRequestAnswersSummarySection>();

        testDataShareRequestAnswersSummary.SummarySections = testSummarySections;

        var result = testDataShareRequestAnswersSummary.SummarySections;

        Assert.That(result, Is.EqualTo(testSummarySections));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetSummarySections_ThenSummarySectionsIsSet()
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        var testSummarySections = new List<DataShareRequestAnswersSummarySection>{ new(), new(), new() };

        testDataShareRequestAnswersSummary.SummarySections = testSummarySections;

        var result = testDataShareRequestAnswersSummary.SummarySections;

        Assert.That(result, Is.EqualTo(testSummarySections));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetSubmissionResponseFromSupplier_ThenSubmissionResponseFromSupplierIsSet(
        [Values(null, "", "  ", "abc")] string? testSubmissionResponseFromSupplier)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.SubmissionResponseFromSupplier = testSubmissionResponseFromSupplier;

        var result = testDataShareRequestAnswersSummary.SubmissionResponseFromSupplier;

        Assert.That(result, Is.EqualTo(testSubmissionResponseFromSupplier));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummary_WhenISetCancellationReasonsFromAcquirer_ThenCancellationReasonsFromAcquirerIsSet(
        [Values(null, "", "  ", "abc")] string? testCancellationReasonsFromAcquirer)
    {
        var testDataShareRequestAnswersSummary = new DataShareRequestAnswersSummary();

        testDataShareRequestAnswersSummary.CancellationReasonsFromAcquirer = testCancellationReasonsFromAcquirer;

        var result = testDataShareRequestAnswersSummary.CancellationReasonsFromAcquirer;

        Assert.That(result, Is.EqualTo(testCancellationReasonsFromAcquirer));
    }
}
