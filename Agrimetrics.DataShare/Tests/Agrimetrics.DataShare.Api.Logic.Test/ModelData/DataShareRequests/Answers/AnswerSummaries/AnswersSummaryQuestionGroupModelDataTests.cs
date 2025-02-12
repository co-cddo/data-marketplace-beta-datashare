using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        var testDataShareRequestId = new Guid("98202589-3A5E-4C27-84D9-2B45AB57D623");

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_EsdaName = testEsdaName;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestId = testRequestId;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestStatus = testRequestStatus;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetAnEmptySetOfSummarySections_ThenSummarySectionsIsSet()
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        var testSummarySections = new List<DataShareRequestAnswersSummarySectionModelData>();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections = testSummarySections;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections;

        Assert.That(result, Is.EqualTo(testSummarySections));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetSummarySections_ThenSummarySectionsIsSet()
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        var testSummarySections = new List<DataShareRequestAnswersSummarySectionModelData> { new(), new(), new() };

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections = testSummarySections;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections;

        Assert.That(result, Is.EqualTo(testSummarySections));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetSubmissionResponseFromSupplier_ThenSubmissionResponseFromSupplierIsSet(
        [Values(null, "", "  ", "abc")] string? testSubmissionResponseFromSupplier)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier = testSubmissionResponseFromSupplier;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier;

        Assert.That(result, Is.EqualTo(testSubmissionResponseFromSupplier));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryModelData_WhenISetCancellationReasonsFromAcquirer_ThenCancellationReasonsFromAcquirerIsSet(
        [Values(null, "", "  ", "abc")] string? testCancellationReasonsFromAcquirer)
    {
        var testDataShareRequestAnswersSummaryModelData = new DataShareRequestAnswersSummaryModelData();

        testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer = testCancellationReasonsFromAcquirer;

        var result = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer;

        Assert.That(result, Is.EqualTo(testCancellationReasonsFromAcquirer));
    }
}
[TestFixture]
public class DataShareRequestAnswersSummaryQuestionGroupModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetMainQuestionId_ThenMainQuestionIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testMainQuestionId = new Guid("2FA293BD-A8E2-45D6-BE3A-7A18B6E632AA");

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId = testMainQuestionId;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId;

        Assert.That(result, Is.EqualTo(testMainQuestionId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetOrderWithinSection_ThenOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSection)
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection = testOrderWithinSection;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection;

        Assert.That(result, Is.EqualTo(testOrderWithinSection));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetAnEmptySetOfBackingQuestionIds_ThenBackingQuestionIdsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testBackingQuestionIds = new List<Guid>();

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds = testBackingQuestionIds;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds;

        Assert.That(result, Is.EqualTo(testBackingQuestionIds));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetBackingQuestionIds_ThenBackingQuestionIdsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testBackingQuestionIds = new List<Guid>{ new Guid("E14A4E30-1B79-4AE3-91E8-D823275EC57F"), new Guid("36413EE4-5C7D-4E12-BEA8-2A773D61C997")};

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds = testBackingQuestionIds;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds;

        Assert.That(result, Is.EqualTo(testBackingQuestionIds));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetSummaryMainQuestion_ThenSummaryMainQuestionIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testSummaryMainQuestion = new DataShareRequestAnswersSummaryQuestionModelData();

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion = testSummaryMainQuestion;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion;

        Assert.That(result, Is.EqualTo(testSummaryMainQuestion));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetAnEmptySetOfSummaryBackingQuestions_ThenSummaryBackingQuestionsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testSummaryBackingQuestions = new List<DataShareRequestAnswersSummaryQuestionModelData>();

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions = testSummaryBackingQuestions;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions;

        Assert.That(result, Is.EqualTo(testSummaryBackingQuestions));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroupModelData_WhenISetSummaryBackingQuestions_ThenSummaryBackingQuestionsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroupModelData = new DataShareRequestAnswersSummaryQuestionGroupModelData();

        var testSummaryBackingQuestions = new List<DataShareRequestAnswersSummaryQuestionModelData> {new(), new(), new()};

        testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions = testSummaryBackingQuestions;

        var result = testDataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions;

        Assert.That(result, Is.EqualTo(testSummaryBackingQuestions));
    }
}