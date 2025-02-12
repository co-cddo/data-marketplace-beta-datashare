using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData;

internal class SupplierDataShareRequestModelDataFactory : ISupplierDataShareRequestModelDataFactory
{
    SubmissionSummariesSet ISupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
        IEnumerable<PendingSubmissionSummaryModelData> pendingSubmissionSummaryModelDatas,
        IEnumerable<CompletedSubmissionSummaryModelData> completedSubmissionSummaryModelDatas)
    {
        ArgumentNullException.ThrowIfNull(pendingSubmissionSummaryModelDatas);
        ArgumentNullException.ThrowIfNull(completedSubmissionSummaryModelDatas);

        return new SubmissionSummariesSet
        {
            PendingSubmissionSummaries = pendingSubmissionSummaryModelDatas.Select(ConvertPendingSubmissionSummary).ToList(),
            CompletedSubmissionSummaries = completedSubmissionSummaryModelDatas.Select(ConvertCompletedSubmissionSummary).ToList()
        };

        PendingSubmissionSummary ConvertPendingSubmissionSummary(PendingSubmissionSummaryModelData pendingSubmissionSummaryModelData)
        {
            return new PendingSubmissionSummary
            {
                DataShareRequestId = pendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestId,
                DataShareRequestRequestId = pendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestRequestId,
                AcquirerOrganisationName = pendingSubmissionSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationName,
                EsdaName = pendingSubmissionSummaryModelData.PendingSubmissionSummary_EsdaName,
                SubmittedOn = pendingSubmissionSummaryModelData.PendingSubmissionSummary_SubmittedOn,
                WhenNeededBy = pendingSubmissionSummaryModelData.PendingSubmissionSummary_WhenNeededBy,
                RequestStatus = DoConvertDataShareRequestStatus(pendingSubmissionSummaryModelData.PendingSubmissionSummary_RequestStatus)
            };
        }

        CompletedSubmissionSummary ConvertCompletedSubmissionSummary(CompletedSubmissionSummaryModelData completedSubmissionSummaryModelData)
        {
            return new CompletedSubmissionSummary
            {
                DataShareRequestId = completedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestId,
                DataShareRequestRequestId = completedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestRequestId,
                AcquirerOrganisationName = completedSubmissionSummaryModelData.CompletedSubmissionSummary_AcquirerOrganisationName,
                EsdaName = completedSubmissionSummaryModelData.CompletedSubmissionSummary_EsdaName,
                SubmittedOn = completedSubmissionSummaryModelData.CompletedSubmissionSummary_SubmittedOn,
                CompletedOn = completedSubmissionSummaryModelData.CompletedSubmissionSummary_CompletedOn,
                Decision = DoConvertSubmissionDecision(completedSubmissionSummaryModelData.CompletedSubmissionSummary_Decision)
            };
        }
    }

    SubmissionInformation ISupplierDataShareRequestModelDataFactory.CreateSubmissionInformation(
        SubmissionInformationModelData submissionInformationModelData)
    {
        return new SubmissionInformation
        {
            DataShareRequestId = submissionInformationModelData.SubmissionInformation_DataShareRequestId,
            DataShareRequestRequestId = submissionInformationModelData.SubmissionInformation_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(submissionInformationModelData.SubmissionInformation_RequestStatus),
            EsdaName = submissionInformationModelData.SubmissionInformation_EsdaName,
            AcquirerOrganisationName = submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName,
            DataTypes = submissionInformationModelData.SubmissionInformation_DataTypes.ToList(),
            ProjectAims = submissionInformationModelData.SubmissionInformation_ProjectAims,
            WhenNeededBy = submissionInformationModelData.SubmissionInformation_WhenNeededBy,
            SubmittedOn = submissionInformationModelData.SubmissionInformation_SubmittedOn,
            AcquirerEmailAddress = submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress,
            AnswerHighlights = submissionInformationModelData.SubmissionInformation_AnswerHighlights.ToList()
        };
    }

    SubmissionDetails ISupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(
        SubmissionDetailsModelData submissionDetailsModelData)
    {
        ArgumentNullException.ThrowIfNull(submissionDetailsModelData);

        return DoCreateSubmissionDetails(submissionDetailsModelData);
    }

    SubmissionReviewInformation ISupplierDataShareRequestModelDataFactory.CreateSubmissionReviewInformation(
        SubmissionReviewInformationModelData submissionReviewInformationModelData)
    {
        ArgumentNullException.ThrowIfNull(submissionReviewInformationModelData);

        var submissionDetails = DoCreateSubmissionDetails(submissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails);

        return new SubmissionReviewInformation
        {
            SubmissionDetails = submissionDetails,
            SupplierNotes = submissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes
        };
    }

    ReturnedSubmissionInformation ISupplierDataShareRequestModelDataFactory.CreateReturnedSubmissionInformation(
        ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData)
    {
        ArgumentNullException.ThrowIfNull(returnedSubmissionInformationModelData);

        return new ReturnedSubmissionInformation
        {
            DataShareRequestId = returnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId,
            DataShareRequestRequestId = returnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(returnedSubmissionInformationModelData.ReturnedSubmission_RequestStatus),
            AcquirerOrganisationName = returnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName,
            EsdaName = returnedSubmissionInformationModelData.ReturnedSubmission_EsdaName,
            SubmittedOn = returnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn,
            ReturnedOn = returnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn,
            WhenNeededBy = returnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy,
            SupplierNotes = returnedSubmissionInformationModelData.ReturnedSubmission_SupplierNotes,
            FeedbackProvided = returnedSubmissionInformationModelData.ReturnedSubmission_FeedbackProvided,
        };
    }
    
    CompletedSubmissionInformation ISupplierDataShareRequestModelDataFactory.CreateCompletedSubmissionInformation(
        CompletedSubmissionInformationModelData completedSubmissionInformationModelData)
    {
        ArgumentNullException.ThrowIfNull(completedSubmissionInformationModelData);

        return new CompletedSubmissionInformation
        {
            DataShareRequestId = completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId,
            DataShareRequestRequestId = completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestStatus),
            Decision = DoConvertSubmissionDecision(completedSubmissionInformationModelData.CompletedSubmission_Decision),
            AcquirerUserEmail = completedSubmissionInformationModelData.CompletedSubmission_AcquirerUserEmailAddress,
            AcquirerOrganisationName = completedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName,
            EsdaName = completedSubmissionInformationModelData.CompletedSubmission_EsdaName,
            SubmittedOn = completedSubmissionInformationModelData.CompletedSubmission_SubmittedOn,
            CompletedOn = completedSubmissionInformationModelData.CompletedSubmission_CompletedOn,
            WhenNeededBy = completedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy,
            SupplierNotes = completedSubmissionInformationModelData.CompletedSubmission_SupplierNotes,
            FeedbackProvided = completedSubmissionInformationModelData.CompletedSubmission_FeedbackProvided
        };
    }

    DataShareRequestAcceptanceResult ISupplierDataShareRequestModelDataFactory.CreateDataShareRequestAcceptanceResult(
        AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData,
        bool? notificationsSentSuccess)
    {
        ArgumentNullException.ThrowIfNull(acceptedDecisionSummaryModelData);

        var acceptedDecisionSummary = new AcceptedDecisionSummary
        {
            DataShareRequestId = acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId,
            DataShareRequestRequestId = acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(acceptedDecisionSummaryModelData.AcceptedDecisionSummary_RequestStatus),
            AcquirerUserEmailAddress = acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress,
            AcquirerOrganisationName = acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName
        };

        return new DataShareRequestAcceptanceResult
        {
            DataShareRequestId = acceptedDecisionSummary.DataShareRequestId,
            AcceptedDecisionSummary = acceptedDecisionSummary,
            NotificationSuccess = DoConvertNotificationSuccess(notificationsSentSuccess)
        };
    }

    DataShareRequestRejectionResult ISupplierDataShareRequestModelDataFactory.CreateDataShareRequestRejectionResult(
        RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData,
        bool? notificationsSentSuccess)
    {
        ArgumentNullException.ThrowIfNull(rejectedDecisionSummaryModelData);

        var rejectedDecisionSummary = new RejectedDecisionSummary
        {
            DataShareRequestId = rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId,
            DataShareRequestRequestId = rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(rejectedDecisionSummaryModelData.RejectedDecisionSummary_RequestStatus),
            AcquirerOrganisationName = rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName
        };

        return new DataShareRequestRejectionResult
        {
            DataShareRequestId = rejectedDecisionSummary.DataShareRequestId,
            RejectedDecisionSummary = rejectedDecisionSummary,
            NotificationSuccess = DoConvertNotificationSuccess(notificationsSentSuccess)
        };
    }

    DataShareRequestReturnResult ISupplierDataShareRequestModelDataFactory.CreateDataShareRequestReturnResult(
        ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData,
        bool? notificationsSentSuccess)
    {
        ArgumentNullException.ThrowIfNull(returnedDecisionSummaryModelData);

        var returnedDecisionSummary = new ReturnedDecisionSummary()
        {
            DataShareRequestId = returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId,
            DataShareRequestRequestId = returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(returnedDecisionSummaryModelData.ReturnedDecisionSummary_RequestStatus),
            AcquirerOrganisationName = returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName
        };

        return new DataShareRequestReturnResult
        {
            DataShareRequestId = returnedDecisionSummary.DataShareRequestId,
            ReturnedDecisionSummary = returnedDecisionSummary,
            NotificationSuccess = DoConvertNotificationSuccess(notificationsSentSuccess)
        };
    }

    private static SubmissionDetails DoCreateSubmissionDetails(
        SubmissionDetailsModelData submissionDetailsModelData)
    {
        return new SubmissionDetails
        {
            DataShareRequestId = submissionDetailsModelData.SubmissionDetails_DataShareRequestId,
            DataShareRequestRequestId = submissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId,
            RequestStatus = DoConvertDataShareRequestStatus(submissionDetailsModelData.SubmissionDetails_RequestStatus),
            EsdaName = submissionDetailsModelData.SubmissionDetails_EsdaName,
            AcquirerOrganisationName = submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName,
            Sections = submissionDetailsModelData.SubmissionDetails_Sections.Select(ConvertSubmissionDetailsSection).ToList(),
            SubmissionReturnDetailsSet = ConvertSubmissionReturnDetailsSet()
        };

        SubmissionReturnDetailsSet ConvertSubmissionReturnDetailsSet()
        {
            return new SubmissionReturnDetailsSet
            {
                SubmissionReturns = submissionDetailsModelData.SubmissionDetails_SubmissionReturnComments.Select(ConvertSubmissionReturnDetails).ToList()
            };

            SubmissionReturnDetails ConvertSubmissionReturnDetails(SubmissionReturnCommentsModelData submissionReturnCommentsModelData)
            {
                return new SubmissionReturnDetails
                {
                    ReturnedOnUtc = submissionReturnCommentsModelData.ReturnedOnUtc,
                    ReturnComments = submissionReturnCommentsModelData.Comments
                };
            }
        }

        SubmissionDetailsSection ConvertSubmissionDetailsSection(SubmissionDetailsSectionModelData submissionDetailsSectionModelData)
        {
            return new SubmissionDetailsSection
            {
                SectionNumber = submissionDetailsSectionModelData.SubmissionDetailsSection_SectionNumber,
                SectionHeader = submissionDetailsSectionModelData.SubmissionDetailsSection_SectionHeader,
                AnswerGroups = submissionDetailsSectionModelData.SubmissionDetailsSection_Questions.Select(ConvertMainQuestionToAnswerGroup).ToList()
            };
        }

        SubmissionDetailsAnswerGroup ConvertMainQuestionToAnswerGroup(SubmissionDetailsMainQuestionModelData mainQuestionModelData)
        {
            return new SubmissionDetailsAnswerGroup
            {
                MainQuestionHeader = mainQuestionModelData.SubmissionDetailsMainQuestion_QuestionHeader,
                OrderWithinSubmission = mainQuestionModelData.SubmissionDetailsMainQuestion_OrderWithinSection,
                MainEntry = ConvertAnswerPartsToGroupEntry(mainQuestionModelData.SubmissionDetailsMainQuestion_AnswerParts),
                BackingEntries = mainQuestionModelData.SubmissionDetailsMainQuestion_BackingQuestions.Select(x =>
                    ConvertAnswerPartsToGroupEntry(x.SubmissionDetailsBackingQuestion_AnswerParts)).ToList()
            };
        }

        SubmissionAnswerGroupEntry ConvertAnswerPartsToGroupEntry(IEnumerable<SubmissionDetailsAnswerPartModelData> answerPartModelDatas)
        {
            return new SubmissionAnswerGroupEntry
            {
                EntryParts = answerPartModelDatas.Select(ConvertAnswerPartToGroupEntryPart).ToList()
            };
        }

        SubmissionAnswerGroupEntryPart ConvertAnswerPartToGroupEntryPart(SubmissionDetailsAnswerPartModelData answerPartModelData)
        {
            return new SubmissionAnswerGroupEntryPart
            {
                OrderWithinGroupEntry = answerPartModelData.SubmissionDetailsAnswerPart_OrderWithinAnswer,
                QuestionPartText = answerPartModelData.SubmissionDetailsAnswerPart_QuestionPartText,
                ResponseInputType = DoConvertResponseInputType(answerPartModelData.SubmissionDetailsAnswerPart_InputType),
                ResponseFormatType = DoConvertResponseFormatType(answerPartModelData.SubmissionDetailsAnswerPart_FormatType),
                MultipleResponsesAllowed = answerPartModelData.SubmissionDetailsAnswerPart_MultipleResponsesAllowed,
                CollectionDescriptionIfMultipleResponsesAllowed = answerPartModelData.SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed,
                Responses = BuildResponses()
            };

            List<SubmissionDetailsAnswerResponse> BuildResponses()
            {
                // Note: There should only ever be exactly one response item per response.  However, in early development there was an issue whereby
                // selection option responses were being reported incorrectly, and if N options were selected then N responses were received, with
                // each containing the full N selected options.
                // Therefore, to cater for some of the bad data that may remain from those early stages, a list is returned but only
                // the first response is actually used

                var responseInputType = answerPartModelData.SubmissionDetailsAnswerPart_InputType;

                if (answerPartModelData.SubmissionDetailsAnswerPart_MultipleResponsesAllowed)
                {
                    return answerPartModelData.SubmissionDetailsAnswerPart_Responses.Select(answerPartResponse =>
                        ConvertAnswerPartResponse(answerPartResponse, responseInputType)).ToList();
                }

                var singleAnswerPartResponse = ConvertAnswerPartResponse(answerPartModelData.SubmissionDetailsAnswerPart_Responses[0], responseInputType);
                return [singleAnswerPartResponse];
            }
        }

        SubmissionDetailsAnswerResponse ConvertAnswerPartResponse(
            SubmissionDetailsAnswerPartResponseModelData submissionDetailsAnswerPartResponse,
            QuestionPartResponseInputType responseInputType)
        {
            return new SubmissionDetailsAnswerResponse
            {
                OrderWithinAnswer = submissionDetailsAnswerPartResponse.SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart,
                ResponseItem = ConvertSubmissionAnswerResponseItem(submissionDetailsAnswerPartResponse.SubmissionDetailsAnswerPartResponse_ResponseItems[0], responseInputType)
            };
        }

        SubmissionDetailsAnswerResponseItemBase ConvertSubmissionAnswerResponseItem(
            SubmissionDetailsAnswerPartResponseItemModelData submissionDetailsAnswerPartResponseItemModelData,
            QuestionPartResponseInputType responseInputType)
        {
            return responseInputType == QuestionPartResponseInputType.FreeForm
                ? ConvertSubmissionAnswerResponseItemFreeForm(submissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_FreeFormData!)
                : ConvertSubmissionAnswerResponseItemOptionSelection(submissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_SelectionOptionData!);
        }

        SubmissionDetailsAnswerResponseItemFreeForm ConvertSubmissionAnswerResponseItemFreeForm(
            SubmissionDetailsAnswerPartResponseItemFreeFormModelData submissionAnswerResponseItemFreeFormModelData)
        {
            return new SubmissionDetailsAnswerResponseItemFreeForm
            {
                AnswerValue = submissionAnswerResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue,
                ValueEntryDeclined = submissionAnswerResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined
            };
        }

        SubmissionDetailsAnswerResponseItemOptionSelection ConvertSubmissionAnswerResponseItemOptionSelection(
            SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData submissionAnswerResponseItemOptionSelectionModelData)
        {
            return new SubmissionDetailsAnswerResponseItemOptionSelection
            {
                SelectedOptions = submissionAnswerResponseItemOptionSelectionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions.Select(ConvertSubmissionAnswerResponseItemSelectedOption).ToList()
            };
        }

        SubmissionDetailsAnswerResponseItemSelectedOption ConvertSubmissionAnswerResponseItemSelectedOption(
            SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData submissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData)
        {
            return new SubmissionDetailsAnswerResponseItemSelectedOption
            {
                OrderWithinSelectedOptions = submissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OrderWithinSelectedOptions,
                SelectionOptionText = submissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OptionValueText,
                SupplementaryAnswerText = submissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_SupplementaryAnswerText
            };
        }
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType DoConvertResponseInputType(
           QuestionPartResponseInputType questionPartResponseInputType)
    {
        return questionPartResponseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm,
            QuestionPartResponseInputType.OptionSelection => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection,
            QuestionPartResponseInputType.None => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.None,
            _ => throw new InvalidEnumValueException("Unknown ResponseInputType")
        };
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType DoConvertResponseFormatType(
        QuestionPartResponseFormatType questionPartResponseFormatType)
    {
        return questionPartResponseFormatType switch
        {
            QuestionPartResponseFormatType.Text => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Text,
            QuestionPartResponseFormatType.Numeric => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Numeric,
            QuestionPartResponseFormatType.Date => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Date,
            QuestionPartResponseFormatType.Time => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Time,
            QuestionPartResponseFormatType.DateTime => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.DateTime,
            QuestionPartResponseFormatType.SelectSingle => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.SelectSingle,
            QuestionPartResponseFormatType.SelectMulti => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.SelectMulti,
            QuestionPartResponseFormatType.ReadOnly => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.ReadOnly,
            QuestionPartResponseFormatType.Country => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Country,
            _ => throw new InvalidEnumValueException("Unknown ResponseFormatType")
        };
    }

    private static DataShareRequestStatus DoConvertDataShareRequestStatus(DataShareRequestStatusType dataShareRequestStatusType)
    {
        return dataShareRequestStatusType switch
        {
            DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
            DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
            DataShareRequestStatusType.Accepted => DataShareRequestStatus.Accepted,
            DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
            DataShareRequestStatusType.Cancelled => DataShareRequestStatus.Cancelled,
            DataShareRequestStatusType.Returned => DataShareRequestStatus.Returned,
            DataShareRequestStatusType.InReview => DataShareRequestStatus.InReview,
            DataShareRequestStatusType.Deleted => DataShareRequestStatus.Deleted,
            _ => throw new InvalidEnumValueException("Invalid DataShareRequestStatusType provided")
        };
    }

    private static SubmissionDecision DoConvertSubmissionDecision(SubmissionDecisionType submissionDecisionType)
    {
        return submissionDecisionType switch
        {
            SubmissionDecisionType.Accepted => SubmissionDecision.Accepted,
            SubmissionDecisionType.Rejected => SubmissionDecision.Rejected,
            _ => throw new InvalidEnumValueException("Submission has unknown Decision")
        };
    }

    private static NotificationSuccess DoConvertNotificationSuccess(bool? notificationSuccess)
    {
        return notificationSuccess switch
        {
            true => NotificationSuccess.SentSuccessfully,
            false => NotificationSuccess.FailedToSend,
            null => NotificationSuccess.NotSent
        };
    }
}