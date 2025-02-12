using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

internal class DataShareRequestQuestionStatusesDetermination(
    IDataShareRequestQuestionSetCompletenessDetermination dataShareRequestQuestionSetCompletenessDetermination) : IDataShareRequestQuestionStatusesDetermination
{
    public IDataShareRequestQuestionStatusesDeterminationResult DetermineQuestionStatuses(
        DataShareRequestQuestionStatusInformationSetModelData dataShareRequestQuestionStatusInformationSetModelData)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionStatusInformationSetModelData);

        var questionStatusDeterminationResults = dataShareRequestQuestionStatusInformationSetModelData
            .DataShareRequestQuestionStatuses
            .Select(DetermineDataShareRequestQuestionStatusInformation).ToList();

        var questionSetCompletenessResult = dataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetCompleteness(
            questionStatusDeterminationResults.Select(x => x.QuestionSetQuestionStatusData).ToList());

        return new DataShareRequestQuestionStatusesDeterminationResult
        {
            QuestionsRemainThatRequireAResponse = questionSetCompletenessResult.QuestionsRequiringAResponse.Any(),
            QuestionStatusDeterminationResults = questionStatusDeterminationResults
        };

        IDataShareRequestQuestionStatusDeterminationResult DetermineDataShareRequestQuestionStatusInformation(
            DataShareRequestQuestionStatusInformationModelData dataShareRequestQuestionStatusInformationModelData)
        {
            var questionId = dataShareRequestQuestionStatusInformationModelData.DataShareRequestQuestionStatus_QuestionId;
            var previousStatus = dataShareRequestQuestionStatusInformationModelData.QuestionResponseInformation.QuestionResponseInformation_QuestionStatusType;

            var questionStatus = DetermineQuestionStatusType(dataShareRequestQuestionStatusInformationModelData, FindQuestionStatusInformation);

            return new DataShareRequestQuestionStatusDeterminationResult
            {
                QuestionSetQuestionStatusData = new DataShareRequestQuestionSetQuestionStatusDataModel
                {
                    QuestionId = questionId,
                    SectionNumber = dataShareRequestQuestionStatusInformationModelData.QuestionSetQuestionInformation.QuestionSet_SectionNumber,
                    QuestionOrderWithinSection = dataShareRequestQuestionStatusInformationModelData.QuestionSetQuestionInformation.QuestionSet_QuestionOrerWithinSection,
                    QuestionStatus = questionStatus
                },
                PreviousQuestionStatus = previousStatus
            };
        }

        DataShareRequestQuestionStatusInformationModelData FindQuestionStatusInformation(Guid questionId)
        {
            return dataShareRequestQuestionStatusInformationSetModelData.DataShareRequestQuestionStatuses
                .Single(x => x.DataShareRequestQuestionStatus_QuestionId == questionId);
        }
    }

    private static QuestionStatusType DetermineQuestionStatusType(
        DataShareRequestQuestionStatusInformationModelData questionStatusInformation,
        Func<Guid, DataShareRequestQuestionStatusInformationModelData> findQuestionStatusInformationFunc)
    {
        if (QuestionHasPreRequisiteThatRequiresAResponse(questionStatusInformation, findQuestionStatusInformationFunc)) return QuestionStatusType.CannotStartYet;

        if (QuestionApplicabilityIsOverriddenBySelectedOptions(questionStatusInformation)) return QuestionStatusType.NotApplicable;

        if (QuestionDoesNotRequireResponse(questionStatusInformation)) return QuestionStatusType.NoResponseNeeded;

        if (QuestionHasResponse(questionStatusInformation)) return QuestionStatusType.Completed;

        return QuestionStatusType.NotStarted;
    }

    private static bool QuestionHasPreRequisiteThatRequiresAResponse(
        DataShareRequestQuestionStatusInformationModelData questionStatusInformation,
        Func<Guid, DataShareRequestQuestionStatusInformationModelData> findQuestionStatusInformationFunc)
    {
        var preRequisiteQuestionStatuses = questionStatusInformation.QuestionPreRequisites.Select(x =>
        {
            var preRequisiteQuestion = findQuestionStatusInformationFunc(x.QuestionPreRequisite_PreRequisiteQuestionId);

            return DetermineQuestionStatusType(preRequisiteQuestion, findQuestionStatusInformationFunc);
        });

        return preRequisiteQuestionStatuses.Any(x => x is QuestionStatusType.NotStarted or QuestionStatusType.CannotStartYet);
    }

    private static bool QuestionHasResponse(DataShareRequestQuestionStatusInformationModelData questionStatusInformation)
    {
        // If a response has been provided to any part of the question then the question is answered, so just check the first
        return questionStatusInformation
            .QuestionResponseInformation.QuestionResponseInformation_QuestionPartResponses[0]
            .QuestionPartResponse_AnswerPartResponseId != null;
    }

    private static bool QuestionDoesNotRequireResponse(DataShareRequestQuestionStatusInformationModelData questionStatusInformation)
    {
        // A question does not require a response if all parts of the question have None response type,
        // i.e. a question could comprise a ReadOnly part and a FreeForm part, so would still require a response
        return questionStatusInformation.QuestionResponseInformation
            .QuestionResponseInformation_QuestionPartResponses.All(x => x.QuestionPartResponse_ResponseInputType == QuestionPartResponseInputType.None);
    }

    private static bool QuestionApplicabilityIsOverriddenBySelectedOptions(
        DataShareRequestQuestionStatusInformationModelData questionStatusInformation)
    {
        return questionStatusInformation.SelectionOptionQuestionSetQuestionApplicabilityOverrides.Any(QuestionApplicabilityOverrideConditionIsMet);

        static bool QuestionApplicabilityOverrideConditionIsMet(QuestionSetQuestionApplicabilityOverride questionApplicabilityOverride)
        {
            return questionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition switch
            {
                QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsSelected => questionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected,
                QuestionSetQuestionApplicabilityConditionType.QuestionIsNotApplicableIfOptionIsNotSelected => !questionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected,
                _ => throw new InvalidOperationException("Question has unknown Applicability Condition")
            };
        }
    }
}