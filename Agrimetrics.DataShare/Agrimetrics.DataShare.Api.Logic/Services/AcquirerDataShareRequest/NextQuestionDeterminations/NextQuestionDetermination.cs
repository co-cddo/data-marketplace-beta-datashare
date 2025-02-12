using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations;

internal class NextQuestionDetermination : INextQuestionDetermination
{
    Guid? INextQuestionDetermination.DetermineNextQuestion(
        Guid currentQuestionId,
        IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel> questionStatuses)
    {
        ArgumentNullException.ThrowIfNull(questionStatuses);

        var orderedQuestionStatuses = questionStatuses
            .OrderBy(x => x.SectionNumber)
            .ThenBy(x => x.QuestionOrderWithinSection)
            .ToList();

        var thisQuestionStatusIndex = orderedQuestionStatuses.FindIndex(x => x.QuestionId == currentQuestionId);

        if (thisQuestionStatusIndex < 0)
        {
            throw new InvalidOperationException("Current Question Id is not found in given Question Statuses");
        }

        var remainingQuestionStatuses = orderedQuestionStatuses.Skip(thisQuestionStatusIndex + 1).ToList();

        var nextWorkableQuestion = remainingQuestionStatuses.FirstOrDefault(x => QuestionStatusIsWorkable(x.QuestionStatus));

        return nextWorkableQuestion?.QuestionId;

        static bool QuestionStatusIsWorkable(QuestionStatusType questionStatusType)
        {
            var workableQuestionStatuses = new List<QuestionStatusType> { QuestionStatusType.NotStarted, QuestionStatusType.Completed, QuestionStatusType.NoResponseNeeded };

            return workableQuestionStatuses.Contains(questionStatusType);
        }
    }
}