using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

namespace Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement
{
    public interface IQuestionSetPlaceholderReplacementService
    {
        void ReplacePlaceholderDataInQuestionModelData(
            DataShareRequestQuestionModelData questionsSummaryModelData,
            string esdaName);

        void ReplacePlaceholderDataInQuestionSetOutlineModelData(
            QuestionSetOutlineModelData questionSetOutlineModelData,
            string esdaName);

        void ReplacePlaceholderDataInSubmissionDetailsModelData(
            SubmissionDetailsModelData submissionDetailsModelData,
            string esdaName);

        void ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
            DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData,
            string esdaName);

        void ReplacePlaceholderDataInSubmissionReviewInformationModelData(
            SubmissionReviewInformationModelData submissionReviewInformation,
            string esdaName);
    }
}
