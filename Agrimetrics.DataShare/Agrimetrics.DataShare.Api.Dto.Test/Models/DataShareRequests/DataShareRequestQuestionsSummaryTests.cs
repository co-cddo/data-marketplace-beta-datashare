using Agrimetrics.DataShare.Api.Dto.Models.Acquirer;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests;

[TestFixture]
public class DataShareRequestQuestionsSummaryTests
{
    [Test]
    public void GivenADataShareRequestQuestionsSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestQuestionsSummary = new DataShareRequestQuestionsSummary();

        var testDataShareRequestId = new Guid("BA6EB4A8-8D27-47E7-A3F7-3DD514D8C511");

        testDataShareRequestQuestionsSummary.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestQuestionsSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testDataShareRequestQuestionsSummary = new DataShareRequestQuestionsSummary();

        testDataShareRequestQuestionsSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testDataShareRequestQuestionsSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestQuestionsSummary = new DataShareRequestQuestionsSummary();

        testDataShareRequestQuestionsSummary.EsdaName = testEsdaName;

        var result = testDataShareRequestQuestionsSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummary_WhenISetQuestionSetSummary_ThenQuestionSetSummaryIsSet()
    {
        var testDataShareRequestQuestionsSummary = new DataShareRequestQuestionsSummary();

        var testQuestionSetSummary = new QuestionSetSummary
        {
            Id = Guid.Empty,
            AnswersSectionComplete = false,
            SectionSummaries = [],
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First(),
            QuestionsRemainThatRequireAResponse = false,
            SupplierOrganisationName = null,
            SubmissionResponseFromSupplier = null,
            CancellationReasonsFromAcquirer = null,
            AcquirerUserDetails = new AcquirerUserDetails
            {
                OrganisationId = 0,
                DomainId = 0,
                UserId = 0
            }
        };

        testDataShareRequestQuestionsSummary.QuestionSetSummary = testQuestionSetSummary;

        var result = testDataShareRequestQuestionsSummary.QuestionSetSummary;

        Assert.That(result, Is.EqualTo(testQuestionSetSummary));
    }
}