using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights;

internal class AnswerHighlightsService(
    ILogger<AnswerHighlightsService> logger,
    IAnswerHighlightsRepository answerHighlightsRepository) : IAnswerHighlightsService
{
    async Task<IEnumerable<string>> IAnswerHighlightsService.GetDataShareRequestsAnswerHighlightsAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var questionHighlightModelDatas =
                (await answerHighlightsRepository.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(dataShareRequestId)).ToList();

            var selectedOptionsModelData = await answerHighlightsRepository.GetDataShareRequestSelectionOptionsModelDataAsync(dataShareRequestId);

            var selectedOptionIds = selectedOptionsModelData.DataShareRequestSelectionOptions_SelectedOptions
                .Select(x => x.DataShareRequestSelectedOption_OptionSelectionId).ToList();

            return FindAnswerHighlights().ToList();

            IEnumerable<string> FindAnswerHighlights()
            {
                foreach (var questionHighlight in questionHighlightModelDatas)
                {
                    var questionHighlightOption = questionHighlight.QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId;
                    var questionHighlightCondition = questionHighlight.QuestionSetSelectionOptionQuestionHighlight_HighlightCondition;
                    
                    var optionIsSelected = selectedOptionIds.Contains(questionHighlightOption);

                    if (optionIsSelected && questionHighlightCondition == QuestionSetSelectionOptionQuestionHighlightConditionType.QuestionIsHighlightedIfOptionIsSelected)
                    {
                        yield return questionHighlight.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted;
                    }

                    if (!optionIsSelected && questionHighlightCondition == QuestionSetSelectionOptionQuestionHighlightConditionType.QuestionIsHighlightedIfOptionIsNotSelected)
                    {
                        yield return questionHighlight.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetDataShareRequestsAnswerHighlights from AnswerHighlightsRepository";

            logger.LogError(ex, errorMessage);

            throw new DataShareRequestGeneralException(errorMessage, ex);
        }
    }
}