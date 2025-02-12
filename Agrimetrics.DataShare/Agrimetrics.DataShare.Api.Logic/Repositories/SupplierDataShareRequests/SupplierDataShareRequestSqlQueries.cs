using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class SupplierDataShareRequestSqlQueries : ISupplierDataShareRequestSqlQueries
{
    string ISupplierDataShareRequestSqlQueries.GetPendingSubmissionSummaries =>
      @"SELECT
	        [dsr].[Id] AS PendingSubmissionSummary_DataShareRequestId,
	        [dsr].[RequestId] AS PendingSubmissionSummary_DataShareRequestRequestId,
	        [dsr].[AcquirerOrganisation] AS PendingSubmissionSummary_AcquirerOrganisationId,
	        [dsr].[EsdaName] AS PendingSubmissionSummary_EsdaName,
	        [dsr].[RequestStatus] AS PendingSubmissionSummary_RequestStatus
        FROM [DataShareRequest] [dsr]
        WHERE   [dsr].[RequestStatus] IN @PendingSubmissionStatuses
            AND [dsr].[SupplierOrganisation] = @SupplierOrganisationId";

    string ISupplierDataShareRequestSqlQueries.GetCompletedSubmissionSummaries =>
      @"SELECT
	        [dsr].[Id] AS CompletedSubmissionSummary_DataShareRequestId,
	        [dsr].[RequestId] AS CompletedSubmissionSummary_DataShareRequestRequestId,
	        [dsr].[AcquirerOrganisation] AS CompletedSubmissionSummary_AcquirerOrganisationId,
	        [dsr].[EsdaName] AS CompletedSubmissionSummary_EsdaName,
            [dsr].[RequestStatus] AS CompletedSubmissionSummary_Status
        FROM [DataShareRequest] [dsr]
        WHERE [dsr].[RequestStatus] IN @CompletedSubmissionStatuses
            AND [dsr].[SupplierOrganisation] = @SupplierOrganisationId";

    string ISupplierDataShareRequestSqlQueries.GetSubmissionInformationModelData =>
        @"SELECT
	        [dsr].[Id] AS SubmissionInformation_DataShareRequestId,
	        [dsr].[RequestId] AS SubmissionInformation_DataShareRequestRequestId,
	        [dsr].[RequestStatus] AS SubmissionInformation_RequestStatus,
	        [dsr].[EsdaName] AS SubmissionInformation_EsdaName,
	        [dsr].[AcquirerUser] AS SubmissionInformation_AcquirerUserId,
	        [dsr].[AcquirerOrganisation] AS SubmissionInformation_AcquirerOrganisationId
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetReturnedSubmissionInformation =>
      @"SELECT
	        [dsr].[Id] AS ReturnedSubmission_DataShareRequestId,
	        [dsr].[RequestId] AS ReturnedSubmission_DataShareRequestRequestId,
	        [dsr].[RequestStatus] AS ReturnedSubmission_RequestStatus,
	        [dsr].[AcquirerOrganisation] AS ReturnedSubmission_AcquirerOrganisationId,
	        [dsr].[EsdaName] AS ReturnedSubmission_EsdaName,
            [s].[Notes] AS ReturnedSubmission_SupplierNotes
        FROM [dbo].[DataShareRequest] [dsr]
            JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId;";

    string ISupplierDataShareRequestSqlQueries.GetCompletedSubmissionInformation =>
      @"SELECT
	        [dsr].[Id] AS CompletedSubmission_DataShareRequestId,
	        [dsr].[RequestId] AS CompletedSubmission_DataShareRequestRequestId,
            [dsr].[RequestStatus] AS CompletedSubmission_DataShareRequestStatus,
	        [dsr].[AcquirerUser] AS CompletedSubmission_AcquirerUserId,
	        [dsr].[AcquirerOrganisation] AS CompletedSubmission_AcquirerOrganisationId,
	        [dsr].[EsdaName] AS CompletedSubmission_EsdaName,
            [s].[Notes] AS CompletedSubmission_SupplierNotes
        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId;";

    string ISupplierDataShareRequestSqlQueries.GetSubmissionNotes =>
      @"SELECT [s].[Notes]
        FROM [dbo].[DataShareRequest] [dsr]
            JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.SetSubmissionNotes =>
      @"UPDATE [dbo].[Submission]
        SET [Notes] = @Notes
        WHERE [DataShareRequest] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetDataShareRequestStatus =>
        @"SELECT
            [RequestStatus] AS DataShareRequestStatus_RequestStatus,
            [QuestionsRemainThatRequireAResponse] AS DataShareRequestStatus_QuestionsRemainThatRequireAResponse
        FROM [dbo].[DataShareRequest]
        WHERE [Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.UpdateDataShareRequestStatus =>
      @"UPDATE [dbo].[DataShareRequest]
        SET [RequestStatus] = @DataShareRequestStatus
        WHERE [Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetAcceptedDecisionSummary =>
      @"SELECT
	        [dsr].[Id] AS AcceptedDecisionSummary_DataShareRequestId,
	        [dsr].[RequestId] AS AcceptedDecisionSummary_DataShareRequestRequestId,
            [dsr].[RequestStatus] AS AcceptedDecisionSummary_RequestStatus,
	        [dsr].[AcquirerUser] AS AcceptedDecisionSummary_AcquirerUserId,
	        [dsr].[AcquirerOrganisation] AS AcceptedDecisionSummary_AcquirerOrganisationId
        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetRejectedDecisionSummary =>
      @"SELECT
	        [dsr].[Id] AS RejectedDecisionSummary_DataShareRequestId,
	        [dsr].[RequestId] AS RejectedDecisionSummary_DataShareRequestRequestId,
            [dsr].[RequestStatus] AS RejectedDecisionSummary_RequestStatus,	        
            [dsr].[AcquirerOrganisation] AS RejectedDecisionSummary_AcquirerOrganisationId
        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetReturnedDecisionSummary =>
      @"SELECT
	        [dsr].[Id] AS ReturnedDecisionSummary_DataShareRequestId,
	        [dsr].[RequestId] AS ReturnedDecisionSummary_DataShareRequestRequestId,
	        [dsr].[RequestStatus] AS ReturnedDecisionSummary_RequestStatus,
	        [dsr].[AcquirerOrganisation] AS ReturnedDecisionSummary_AcquirerOrganisationId
        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[Submission] [s] ON [s].[DataShareRequest] = [dsr].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string ISupplierDataShareRequestSqlQueries.GetSubmissionDetailsModelDatas =>
      @"SELECT
            -- Submission Details
	        [dsr].[Id] AS SubmissionDetails_DataShareRequestId,
	        [dsr].[RequestId] AS SubmissionDetails_DataShareRequestRequestId,
	        [dsr].[RequestStatus] AS SubmissionDetails_RequestStatus,
	        [dsr].[EsdaName] AS SubmissionDetails_EsdaName,
	        [dsr].[AcquirerOrganisation] AS SubmissionDetails_AcquirerOrganisationId,

	        -- Sections
	        [qss].[Id] AS SubmissionDetailsSection_SectionId,
	        [qss].[SectionNumber] AS SubmissionDetailsSection_SectionNumber,
	        [qss].[SectionHeader] AS SubmissionDetailsSection_SectionHeader,

	        -- Main Questions
	        [q_main].[Id] AS SubmissionDetailsMainQuestion_Id,
	        [qsq_main].[QuestionOrder] AS SubmissionDetailsMainQuestion_OrderWithinSection,
	        [q_main].[Header] AS SubmissionDetailsMainQuestion_QuestionHeader,

	        -- Main Question Answer Parts
	        [ap_main].[Id] AS SubmissionDetailsAnswerPart_Id,
	        [qps_main].[QuestionPartOrder] AS SubmissionDetailsAnswerPart_OrderWithinAnswer,
	        [qp_main].[QuestionText] AS SubmissionDetailsAnswerPart_QuestionPartText,
	        [rft_main].[InputType] AS SubmissionDetailsAnswerPart_InputType,
	        [qp_main].[ResponseFormatType] AS SubmissionDetailsAnswerPart_FormatType,
	        [qp_main].[AllowMultipleAnswerItems] AS SubmissionDetailsAnswerPart_MultipleResponsesAllowed,
	        [qpmari_main].[CollectionDescription] AS SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed,

	        -- Backing Questions
	        [q_backing].[Id] AS SubmissionDetailsBackingQuestion_Id,
	        [qsq_backing].[QuestionOrder] AS SubmissionDetailsMainQuestion_OrderWithinSection,
	        [q_backing].[Header] AS SubmissionDetailsMainQuestion_QuestionHeader,

	        -- Backing Question Answer Parts
	        [ap_backing].[Id] AS SubmissionDetailsAnswerPart_Id,
	        [qps_backing].[QuestionPartOrder] AS SubmissionDetailsAnswerPart_OrderWithinAnswer,
	        [qp_backing].[QuestionText] AS SubmissionDetailsAnswerPart_QuestionPartText,
	        [rft_backing].[InputType] AS SubmissionDetailsAnswerPart_InputType,
	        [qp_backing].[ResponseFormatType] AS SubmissionDetailsAnswerPart_FormatType,
	        [qp_backing].[AllowMultipleAnswerItems] AS SubmissionDetailsAnswerPart_MultipleResponsesAllowed,
	        [qpmari_backing].[CollectionDescription] AS SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed

        FROM [dbo].[DataShareRequest] [dsr]
	        -- Submission
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]

	        -- Main Question
	        JOIN [dbo].[QuestionSetQuestion] [qsq_main] ON [qsq_main].[QuestionSetSection] = [qss].[Id] AND [qsq_main].[FrontingQuestion] IS NULL
	        JOIN [dbo].[Question] [q_main] ON [q_main].[Id] = [qsq_main].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps_main] ON [qps_main].[Question] = [q_main].[Id]
	        JOIN [dbo].[QuestionPart] [qp_main] ON [qp_main].[Id] = [qps_main].[QuestionPart] AND [qps_main].[QuestionPartType] = 'MainQuestionPart'
	        JOIN [dbo].[ResponseFormatType] [rft_main] ON [rft_main].[Id] = [qp_main].[ResponseFormatType]
	        LEFT JOIN [dbo].[QuestionPartMultiAnswerResponseInformation] [qpmari_main] ON [qpmari_main].[QuestionPart] = [qp_main].[Id]
	        JOIN [dbo].[Answer] [a_main] ON [a_main].[AnswerSet] = [as].[Id] AND [a_main].[Question] = [q_main].[Id]
                AND [a_main].[QuestionStatus] != 'NotApplicable'
                AND [a_main].[QuestionStatus] != 'NoResponseNeeded'
	        JOIN [dbo].[AnswerPart] [ap_main] ON [ap_main].[Answer] = [a_main].[Id] AND [ap_main].[QuestionPart] = [qp_main].[Id]

	        -- Backing Questions
	        LEFT JOIN [dbo].[QuestionSetQuestion] [qsq_backing] ON [qsq_backing].[FrontingQuestion] = [q_main].[Id]
	        LEFT JOIN [dbo].[Question] [q_backing] ON [q_backing].[Id] = [qsq_backing].[Question]
	        LEFT JOIN [dbo].[QuestionPartSet] [qps_backing] ON [qps_backing].[Question] = [q_backing].[Id]
	        LEFT JOIN [dbo].[QuestionPart] [qp_backing] ON [qp_backing].[Id] = [qps_backing].[QuestionPart] AND [qps_backing].[QuestionPartType] = 'MainQuestionPart'
	        LEFT JOIN [dbo].[ResponseFormatType] [rft_backing] ON [rft_backing].[Id] = [qp_backing].[ResponseFormatType]
	        LEFT JOIN [dbo].[QuestionPartMultiAnswerResponseInformation] [qpmari_backing] ON [qpmari_backing].[QuestionPart] = [qp_backing].[Id]
	        LEFT JOIN [dbo].[Answer] [a_backing] ON [a_backing].[AnswerSet] = [as].[Id] AND [a_backing].[Question] = [q_backing].[Id]
                AND [a_backing].[QuestionStatus] != 'NotApplicable'
                AND [a_backing].[QuestionStatus] != 'NoResponseNeeded'
	        LEFT JOIN [dbo].[AnswerPart] [ap_backing] ON [ap_backing].[Answer] = [a_backing].[Id] AND [ap_backing].[QuestionPart] = [qp_backing].[Id]
        WHERE [dsr].[Id] = @DataShareRequestId
        ORDER BY
	        [qss].[SectionNumber],
	        [qsq_main].[QuestionOrder],
	        [qps_main].[QuestionPartOrder],
	        [qps_backing].[QuestionPartOrder]";

    string ISupplierDataShareRequestSqlQueries.GetSubmissionDetailsAnswerPartResponseModelDatas =>
      @"SELECT
	        -- Responses
	        [apr].[Id] AS SubmissionDetailsAnswerPartResponse_Id,
	        [apr].[OrderWithinAnswerPart] AS SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart,

	        -- Response Items
	        [apri].[Id] AS SubmissionDetailsAnswerResponseItem_Id,

	        -- Free Form Response Item Data
	        [apriff].[Id] AS SubmissionDetailsAnswerPartResponseItemFreeForm_Id,
	        [apriff].[EnteredValue] AS SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue,
	        [apriff].ValueEntryDeclined AS SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined,

	        -- Selection Option Response Item Data
	        [apriso].[Id] AS SubmissionDetailsAnswerPartResponseItemSelectionOption_Id,

	        -- Selected Options
	        [so].[Id] AS SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_Id,
	        [so].[OptionOrder] AS SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OrderWithinSelectedOptions,
	        [so].[OptionValueText] AS SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OptionValueText,
	        [apriff_supp].[EnteredValue] AS SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_SupplementaryAnswerText

        FROM [dbo].[AnswerPart] [ap]
	        LEFT JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemFreeForm] [apriff] ON [apriff].[Id] = [apri].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemSelectionOption] [apriso] ON [apriso].[AnswerPartResponseItem] = [apri].[Id]
	        LEFT JOIN [dbo].[SelectionOption] [so] ON [so].[Id] = [apriso].[SelectionOption]
	        LEFT JOIN [dbo].[AnswerPart] [apriso_supp] ON [apriso_supp].[Id] = [apriso].[SupplementaryQuestionPartAnswer]
	        LEFT JOIN [dbo].[AnswerPartResponse] [apr_supp] ON [apr_supp].[AnswerPart] = [apriso_supp].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItem] [apri_supp] ON [apri_supp].[AnswerPartResponse] = [apr_supp].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemFreeForm] [apriff_supp] ON [apriff_supp].[Id] = [apri_supp].[Id]
        WHERE [ap].[Id] = @AnswerPartId
        ORDER BY
	        [apr].[OrderWithinAnswerPart],
	        [so].[OptionOrder]";

    string ISupplierDataShareRequestSqlQueries.GetDataShareRequestNotificationInformation =>
        @"SELECT
            [dsr].[SupplierOrganisation] AS SupplierOrganisationId,
	        [dsr].[AcquirerUser] AS AcquirerUserId,
            [dsr].[RequestId] AS DataShareRequestRequestId,
            [dsr].[Esda] AS EsdaId,
	        [dsr].[EsdaName] AS EsdaName
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";
}