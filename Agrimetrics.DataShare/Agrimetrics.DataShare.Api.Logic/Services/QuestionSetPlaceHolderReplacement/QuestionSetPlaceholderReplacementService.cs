using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

namespace Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;

internal class QuestionSetPlaceholderReplacementService : IQuestionSetPlaceholderReplacementService
{
    private const string esdaNamePlaceholder = "[[<<EsdaName>>]]";

    void IQuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(
        DataShareRequestQuestionModelData questionsSummaryModelData,
        string esdaName)
    {
        ArgumentNullException.ThrowIfNull(questionsSummaryModelData);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        var questionPartPrompts = questionsSummaryModelData.DataShareRequestQuestion_QuestionParts
            .Select(x => x.DataShareRequestQuestionPart_Question.QuestionPart_Prompts);

        foreach (var questionPartPrompt in questionPartPrompts)
        {
            var questionHeaderReplacementResult = DoReplacePlaceholderInformationInQuestionTextIfFound(
                questionPartPrompt.QuestionPartPrompt_QuestionText, 
                esdaName);

            if (questionHeaderReplacementResult.PlaceholderReplacementHasOccurred)
            {
                questionPartPrompt.QuestionPartPrompt_QuestionText = questionHeaderReplacementResult.PopulatedValue;
            }
        }
    }

    void IQuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(
        QuestionSetOutlineModelData questionSetOutlineModelData,
        string esdaName)
    {
        ArgumentNullException.ThrowIfNull(questionSetOutlineModelData);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        var questionOutlines = questionSetOutlineModelData.Sections.SelectMany(section =>
            section.Questions.Select(question => question));

        foreach (var questionOutline in questionOutlines)
        {
            var questionHeaderReplacementResult = DoReplacePlaceholderInformationInQuestionTextIfFound(
                questionOutline.QuestionSetQuestionOutline_QuestionText,
                esdaName);

            if (questionHeaderReplacementResult.PlaceholderReplacementHasOccurred)
            {
                questionOutline.QuestionSetQuestionOutline_QuestionText = questionHeaderReplacementResult.PopulatedValue!;
            }
        }
    }

    void IQuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
        SubmissionDetailsModelData submissionDetailsModelData,
        string esdaName)
    {
        ArgumentNullException.ThrowIfNull(submissionDetailsModelData);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        var allAnswerParts = ExtractAllAnswerParts();

        foreach (var answerPart in allAnswerParts)
        {
            var questionHeaderReplacementResult = DoReplacePlaceholderInformationInQuestionTextIfFound(
                answerPart.SubmissionDetailsAnswerPart_QuestionPartText,
                esdaName);

            if (questionHeaderReplacementResult.PlaceholderReplacementHasOccurred)
            {
                answerPart.SubmissionDetailsAnswerPart_QuestionPartText = questionHeaderReplacementResult.PopulatedValue!;
            }
        }

        IEnumerable<SubmissionDetailsAnswerPartModelData> ExtractAllAnswerParts()
        {
            var allQuestions = submissionDetailsModelData.SubmissionDetails_Sections.SelectMany(x => x.SubmissionDetailsSection_Questions).ToList();

            var allMainQuestionAnswerParts = allQuestions.SelectMany(x =>
                x.SubmissionDetailsMainQuestion_AnswerParts);

            var allBackingQuestionAnswerParts = allQuestions.SelectMany(x =>
                x.SubmissionDetailsMainQuestion_BackingQuestions.SelectMany(y => y.SubmissionDetailsBackingQuestion_AnswerParts));

            return allMainQuestionAnswerParts.Concat(allBackingQuestionAnswerParts);
        }
    }

    void IQuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
        DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData,
        string esdaName)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestAnswersSummaryModelData);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        var allQuestionParts = ExtractAllQuestionParts();

        foreach (var questionPart in allQuestionParts)
        {
            var questionPartTextReplacementResult = DoReplacePlaceholderInformationInQuestionTextIfFound(
                questionPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText,
                esdaName);

            if (questionPartTextReplacementResult.PlaceholderReplacementHasOccurred)
            {
                questionPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText = questionPartTextReplacementResult.PopulatedValue!;
            }
        }

        IEnumerable<DataShareRequestAnswersSummaryQuestionPartModelData> ExtractAllQuestionParts()
        {
            var allQuestionGroups = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections.SelectMany(x =>
                x.DataShareRequestAnswersSummarySection_QuestionGroups).ToList();

            var allMainQuestions = allQuestionGroups.Select(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion);

            var allBackingQuestions = allQuestionGroups.SelectMany(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions);

            var allQuestions = allMainQuestions.Concat(allBackingQuestions).ToList();

            return allQuestions.SelectMany(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts);
        }
    }

    void IQuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(
        SubmissionReviewInformationModelData submissionReviewInformation,
        string esdaName)
    {
        ArgumentNullException.ThrowIfNull(submissionReviewInformation);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        var allAnswerParts = ExtractAllAnswerParts();

        foreach (var answerPart in allAnswerParts)
        {
            var answerPartTextReplacementResult = DoReplacePlaceholderInformationInQuestionTextIfFound(
                answerPart.SubmissionDetailsAnswerPart_QuestionPartText,
                esdaName);

            if (answerPartTextReplacementResult.PlaceholderReplacementHasOccurred)
            {
                answerPart.SubmissionDetailsAnswerPart_QuestionPartText = answerPartTextReplacementResult.PopulatedValue!;
            }
        }

        IEnumerable<SubmissionDetailsAnswerPartModelData> ExtractAllAnswerParts()
        {
            var allQuestions = submissionReviewInformation.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_Sections
                .SelectMany(x => x.SubmissionDetailsSection_Questions).ToList();

            var allMainQuestionParts = allQuestions.SelectMany(x => x.SubmissionDetailsMainQuestion_AnswerParts);

            var allBackingQuestionsParts = allQuestions.SelectMany(x =>
                x.SubmissionDetailsMainQuestion_BackingQuestions.SelectMany(y =>
                    y.SubmissionDetailsBackingQuestion_AnswerParts));

            return allMainQuestionParts.Concat(allBackingQuestionsParts);
        }
    }

    private sealed class PlaceholderReplacementResult
    {
        public bool PlaceholderReplacementHasOccurred { get; set; }
        public string? PopulatedValue { get; set; }
    }

    private static PlaceholderReplacementResult DoReplacePlaceholderInformationInQuestionTextIfFound(
        string? questionText,
        string esdaName)
    {
        var placeholderReplacementResult = new PlaceholderReplacementResult
        {
            PlaceholderReplacementHasOccurred = false,
            PopulatedValue = questionText
        };

        DoReplaceResourceNameInSituIfFound();

        return placeholderReplacementResult;

        void DoReplaceResourceNameInSituIfFound()
        {
            if (placeholderReplacementResult.PopulatedValue == null) return;

            var valueContainsResourceName = placeholderReplacementResult.PopulatedValue.Contains(esdaNamePlaceholder);

            if (!valueContainsResourceName) return;

            placeholderReplacementResult.PopulatedValue = placeholderReplacementResult.PopulatedValue.Replace(
                esdaNamePlaceholder, esdaName);

            placeholderReplacementResult.PlaceholderReplacementHasOccurred = true;
        }
    }
}