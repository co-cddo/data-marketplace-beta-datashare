using Agrimetrics.DataShare.Api.Dto.Models.Acquirer;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionSets;

[TestFixture]
public class QuestionSetSummaryTests
{
    [Test]
    public void GivenAQuestionSetSummary_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        var testId = new Guid("D012C618-C008-4809-9CA3-ECDE31D0A479");

        testQuestionSetSummary.Id = testId;

        var result = testQuestionSetSummary.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Theory]
    public void GivenAQuestionSetSummary_WhenISetAnswersSectionComplete_ThenAnswersSectionCompleteIsSet(
        bool testAnswersSectionComplete)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.AnswersSectionComplete = testAnswersSectionComplete;

        var result = testQuestionSetSummary.AnswersSectionComplete;

        Assert.That(result, Is.EqualTo(testAnswersSectionComplete));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetAnEmptySetOfSectionSummaries_ThenSectionSummariesIsSet()
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        var testSectionSummaries = new List<QuestionSetSectionSummary>();

        testQuestionSetSummary.SectionSummaries = testSectionSummaries;

        var result = testQuestionSetSummary.SectionSummaries;

        Assert.That(result, Is.EqualTo(testSectionSummaries));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetSectionSummaries_ThenSectionSummariesIsSet()
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        var testSectionSummaries = new List<QuestionSetSectionSummary> {new(), new(), new()};

        testQuestionSetSummary.SectionSummaries = testSectionSummaries;

        var result = testQuestionSetSummary.SectionSummaries;

        Assert.That(result, Is.EqualTo(testSectionSummaries));
    }

    [Theory]
    public void GivenAQuestionSetSummary_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatus testDataShareRequestStatus)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.DataShareRequestStatus = testDataShareRequestStatus;

        var result = testQuestionSetSummary.DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }

    [Theory]
    public void GivenAQuestionSetSummary_WhenISetQuestionsRemainThatRequireAResponse_ThenQuestionsRemainThatRequireAResponseIsSet(
        bool testQuestionsRemainThatRequireAResponse)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.QuestionsRemainThatRequireAResponse = testQuestionsRemainThatRequireAResponse;

        var result = testQuestionSetSummary.QuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testQuestionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetSupplierOrganisationName_ThenSupplierOrganisationNameIsSet(
        [Values(null, "", "  ", "abc")] string? testSupplierOrganisationName)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.SupplierOrganisationName = testSupplierOrganisationName;

        var result = testQuestionSetSummary.SupplierOrganisationName;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationName));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetSubmissionResponseFromSupplier_ThenSubmissionResponseFromSupplierIsSet(
        [Values(null, "", "  ", "abc")] string? testSubmissionResponseFromSupplier)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.SubmissionResponseFromSupplier = testSubmissionResponseFromSupplier;

        var result = testQuestionSetSummary.SubmissionResponseFromSupplier;

        Assert.That(result, Is.EqualTo(testSubmissionResponseFromSupplier));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetCancellationReasonsFromAcquirer_ThenCancellationReasonsFromAcquirerIsSet(
        [Values(null, "", "  ", "abc")] string? testCancellationReasonsFromAcquirer)
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        testQuestionSetSummary.CancellationReasonsFromAcquirer = testCancellationReasonsFromAcquirer;

        var result = testQuestionSetSummary.CancellationReasonsFromAcquirer;

        Assert.That(result, Is.EqualTo(testCancellationReasonsFromAcquirer));
    }

    [Test]
    public void GivenAQuestionSetSummary_WhenISetAcquirerUserDetails_ThenAcquirerUserDetailsIsSet()
    {
        var testQuestionSetSummary = new QuestionSetSummary();

        var testAcquirerUserDetails = new AcquirerUserDetails
        {
            OrganisationId = 0,
            DomainId = 0,
            UserId = 0
        };

        testQuestionSetSummary.AcquirerUserDetails = testAcquirerUserDetails;

        var result = testQuestionSetSummary.AcquirerUserDetails;

        Assert.That(result, Is.SameAs(testAcquirerUserDetails));
    }
}