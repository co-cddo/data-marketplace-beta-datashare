using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

internal class DataShareRequestQuestionSetCompletenessDetermination : IDataShareRequestQuestionSetCompletenessDetermination
{
    private readonly List<QuestionStatusType> questionStatusesNotRequiringAResponse =
    [
        QuestionStatusType.NoResponseNeeded,
        QuestionStatusType.Completed,
        QuestionStatusType.NotApplicable
    ];

    IDataShareRequestQuestionSetCompletenessDeterminationResult IDataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetCompleteness(
        IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel> questionSetQuestionStatuses)
    {
        ArgumentNullException.ThrowIfNull(questionSetQuestionStatuses);

        var questionsRequiringResponses = questionSetQuestionStatuses
            .Where(questionStatusDetermination => !questionStatusesNotRequiringAResponse.Contains(questionStatusDetermination.QuestionStatus));

        return new DataShareRequestQuestionSetCompletenessDeterminationResult
        {
            QuestionsRequiringAResponse = questionsRequiringResponses
        };
    }

    IDataShareRequestQuestionSetSectionCompletionDeterminationResult IDataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetSectionCompleteness(
        QuestionSetSectionSummaryModelData questionSetSection)
    {
        ArgumentNullException.ThrowIfNull(questionSetSection);

        var questionsRequiringResponses = questionSetSection.QuestionSetSection_QuestionSummaries
            .Where(questionStatusDetermination => !questionStatusesNotRequiringAResponse.Contains(questionStatusDetermination.Question_QuestionStatus));

        return new DataShareRequestQuestionSetSectionCompletionDeterminationResult
        {
            QuestionsRequiringAResponse = questionsRequiringResponses
        };
    }
}