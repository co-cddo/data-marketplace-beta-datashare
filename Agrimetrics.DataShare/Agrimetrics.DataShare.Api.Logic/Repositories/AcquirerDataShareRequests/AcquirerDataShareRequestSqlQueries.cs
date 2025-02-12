using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class AcquirerDataShareRequestSqlQueries : IAcquirerDataShareRequestSqlQueries
{
    string IAcquirerDataShareRequestSqlQueries.GetMasterQuestionSetId =>
      @"SELECT [qs].[Id]
        FROM [dbo].[QuestionSet] [qs]
        WHERE   [qs].[SupplierOrganisation] IS NULL
            AND [qs].[SupplierDomain] IS NULL
            AND [qs].[Esda] IS NULL";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionSetIdForEsdaAndSupplierDomain =>
      @"SELECT [qs].[Id]
        FROM [dbo].[QuestionSet] [qs]
        WHERE   [qs].[SupplierDomain] = @SupplierDomainId
            AND [qs].[SupplierOrganisation] = @SupplierOrganisationId
	        AND [qs].[Esda] = @EsdaId";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionSetIdForEsdaAndSupplierOrganisation =>
      @"SELECT [qs].[Id]
        FROM [dbo].[QuestionSet] [qs]
        WHERE   [qs].[SupplierDomain] IS NULL
            AND [qs].[SupplierOrganisation] = @SupplierOrganisationId
	        AND [qs].[Esda] = @EsdaId";

    string IAcquirerDataShareRequestSqlQueries.GetDefaultQuestionSetIdForSupplierDomain =>
      @"SELECT [qs].[Id]
        FROM [dbo].[QuestionSet] [qs]
        WHERE   [qs].[SupplierDomain] = @SupplierDomainId
            AND [qs].[SupplierOrganisation] = @SupplierOrganisationId
	        AND [qs].[Esda] IS NULL";

    string IAcquirerDataShareRequestSqlQueries.GetDefaultQuestionSetIdForSupplierOrganisation =>
      @"SELECT [qs].[Id]
        FROM [dbo].[QuestionSet] [qs]
        WHERE   [qs].[SupplierDomain] IS NULL
            AND [qs].[SupplierOrganisation] = @SupplierOrganisationId
	        AND [qs].[Esda] IS NULL";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionSetOutlineRequest =>
      @"SELECT
	        [qs].[Id] AS QuestionSetOutline_Id,
	        [qs].[Esda] AS QuestionSetOutline_EsdaId,
	        [qs].[SupplierDomain] AS QuestionSetOutline_SupplierDomain,
	        [qs].[SupplierOrganisation] AS QuestionSetOutline_SupplierOrganisation,

	        [qss].[Id] AS QuestionSetSectionOutline_Id,
	        [qss].[SectionNumber] AS QuestionSetSectionOutline_OrderWithinQuestionSetOutline,
	        [qss].[SectionHeader] AS QuestionSetSectionOutline_SectionHeader,

	        [qsq].[Id] AS QuestionSetQuestionOutline_Id,
	        [qsq].[QuestionOrder] AS QuestionSetQuestionOutline_OrderWithinSection,
	        [qp].[QuestionText] AS QuestionSetQuestionOutline_QuestionText

        FROM [dbo].[QuestionSet] [qs]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSetSection] = [qss].[Id] AND [qsq].[ShowOnQuestionsPage] = 1
	        JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartOrder] = 1 AND [qps].[QuestionPartType] = 'MainQuestionPart'
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]
        WHERE [qs].[Id] = @QuestionSetId
        ORDER BY
	        [qss].[SectionNumber],
	        [qsq].[QuestionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetEsdaNameForDataShareRequest =>
        @"SELECT [dsr].[EsdaName]
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.GetTotalNumberOfDataShareRequests =>
      @"SELECT COUNT(*)
        FROM [dbo].[DataShareRequest]";

    string IAcquirerDataShareRequestSqlQueries.CreateDataShareRequest =>
      @"INSERT INTO [dbo].[DataShareRequest] (AcquirerUser, AcquirerDomain, AcquirerOrganisation, Esda, EsdaName, SupplierDomain, SupplierOrganisation, QuestionSet, RequestId)
        OUTPUT [inserted].[Id]
        VALUES (
		    @AcquirerUserId,
            @AcquirerDomainId,
		    @AcquirerOrganisationId,
		    @EsdaId,
            @EsdaName,
            @SupplierDomainId,
		    @SupplierOrganisationId,
		    @QuestionSetId,
            @RequestId)";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestRequestId =>
      @"SELECT [dsr].[RequestId]
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerSet =>
      @"INSERT INTO [dbo].[AnswerSet] (DataShareRequest)
        OUTPUT [inserted].[Id]
        VALUES (
            @DataShareRequestId)";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswer =>
      @"INSERT INTO [dbo].[Answer] (AnswerSet, Question)
        OUTPUT [inserted].[Id]
        VALUES (
	        @AnswerSetId,
	        @QuestionId)";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerPart =>
      @"INSERT INTO [dbo].[AnswerPart] (Answer, QuestionPart)
        VALUES (
	        @AnswerId,
	        @QuestionPartId)";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestStatus =>
      @"SELECT
        [RequestStatus] AS DataShareRequestStatus_RequestStatus,
        [QuestionsRemainThatRequireAResponse] AS DataShareRequestStatus_QuestionsRemainThatRequireAResponse

        FROM [dbo].[DataShareRequest]
        WHERE [Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestAnswerSetId =>
      @"SELECT [Id]
        FROM [dbo].[AnswerSet]
        WHERE DataShareRequest = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.UpdateDataShareRequestQuestionStatus =>
      @"UPDATE [dbo].[Answer]
        SET [QuestionStatus] = @QuestionStatus
        WHERE   [AnswerSet] = @AnswerSetId
	        AND [Question] = @QuestionId";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionSetQuestionParts =>
      @"SELECT
	        [q].[Id] AS QuestionSet_QuestionId,
	        [qp].[Id] AS QuestionSet_QuestionPartId

        FROM [dbo].[QuestionSet] qs
	        JOIN [dbo].[QuestionSetSection] qss ON [qss].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[QuestionSetQuestion] qsq ON [qsq].[QuestionSetSection] = [qss].[Id]
	        JOIN [dbo].[Question] q ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] qps ON [qps].[Question] = [q].[Id]
	        JOIN [dbo].[QuestionPart] qp ON [qp].[Id] = [qps].[QuestionPart]
        WHERE [qs].[Id] = @QuestionSetId
        ORDER BY
	        qss.SectionNumber,
	        qsq.QuestionOrder,
	        qps.QuestionPartOrder";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestModelData =>
      @"SELECT
            [dsr].[Id] AS DataShareRequest_Id,
            [dsr].[RequestId] AS DataShareRequest_RequestId,
            [dsr].[AcquirerUser] AS DataShareRequest_AcquirerUserId,
            [dsr].[AcquirerDomain] AS DataShareRequest_AcquirerDomainId,
            [dsr].[AcquirerOrganisation] AS DataShareRequest_AcquirerOrganisationId,
            [dsr].[SupplierOrganisation] AS DataShareRequest_SupplierOrganisationId,
            [dsr].[Esda] AS DataShareRequest_EsdaId,            
            [dsr].[EsdaName] AS DataShareRequest_EsdaName,
            [dsr].[RequestStatus] AS DataShareRequest_RequestStatus,
            [dsr].[QuestionSet] AS DataShareRequest_QuestionSetId,
	        [dsr].[RequestStatus] AS DataShareRequest_DataShareRequestStatus,
	        [dsr].[QuestionsRemainThatRequireAResponse] AS DataShareRequest_QuestionsRemainThatRequireAResponse
        FROM [dbo].[DataShareRequest] dsr
        WHERE (@AcquirerUserId IS NULL OR [dsr].[AcquirerUser] = @AcquirerUserId)
            AND (@AcquirerDomainId IS NULL OR [dsr].[AcquirerDomain] = @AcquirerDomainId)            
            AND (@AcquirerOrganisationId IS NULL OR [dsr].[AcquirerOrganisation] = @AcquirerOrganisationId)
            AND (@EsdaId IS NULL OR [dsr].[Esda] = @EsdaId)
            AND (@SupplierDomainId IS NULL OR [dsr].[SupplierDomain] = @SupplierDomainId)
            AND (@SupplierOrganisationId IS NULL OR [dsr].[SupplierOrganisation] = @SupplierOrganisationId)
            AND (@HasDataShareRequestStatuses = 0 OR [dsr].[RequestStatus] IN @DataShareRequestStatuses)
		ORDER BY [dsr].[RequestId]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionsSummaryModelData =>
      @"SELECT
            [dsr].[Id] AS DataShareRequest_Id,
            [dsr].[RequestId] AS DataShareRequest_RequestId,
            [dsr].[AcquirerUser] AS DataShareRequest_AcquirerUserId,
            [dsr].[AcquirerDomain] AS DataShareRequest_AcquirerDomainId,
            [dsr].[AcquirerOrganisation] AS DataShareRequest_AcquirerOrganisationId,
            [dsr].[EsdaName] AS DataShareRequest_EsdaName,
			[dsr].[SupplierOrganisation] AS DataShareRequest_SupplierOrganisationId,
			[dsr].[RequestStatus] AS DataShareRequest_DataShareRequestStatus,
			[dsr].[QuestionsRemainThatRequireAResponse] AS DataShareRequest_QuestionsRemainThatRequireAResponse,

	        [qs].[Id] AS QuestionSet_Id,

			[qss].[Id] AS QuestionSetSection_Id,
			[qss].[SectionNumber] AS QuestionSetSection_Number,
			[qss].[SectionHeader] AS QuestionSetSection_Header,

			[q].[Id] AS Question_Id,
            [qsq].[QuestionOrder] AS Question_OrderWithinQuestionSetSection,            
            [q].[Header] AS Question_Header,			            
            [a].[QuestionStatus] AS Question_QuestionStatus

        FROM [dbo].[DataShareRequest] dsr
            JOIN [dbo].[QuestionSet] qs ON [qs].[Id] = [dsr].[QuestionSet]
			JOIN [dbo].[QuestionSetSection] qss ON [qss].[QuestionSet] = [qs].[Id]
			JOIN [dbo].[QuestionSetQuestion] qsq ON [qsq].[QuestionSetSection] = [qss].[Id]
			JOIN [dbo].[Question] q ON [q].[Id] = [qsq].[Question]
			JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
        WHERE   ([dsr].[Id] = @DataShareRequestId)
            AND ([qsq].[ShowOnQuestionsPage] = '1')
        ORDER BY
	        [qss].[SectionNumber],
	        [qsq].[QuestionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionModelData =>
      @"SELECT
            [dsr].[Id] AS DataShareRequestQuestion_DataShareRequestId,
            [dsr].[RequestId] AS DataShareRequestQuestion_DataShareRequestRequestId,
			[q].[Id] AS DataShareRequestQuestion_QuestionId,
			[qsq].[IsOptional] AS DataShareRequestQuestion_IsOptional,

			[qp].[Id] AS QuestionPart_Id,

			[qps].[QuestionPartOrder] AS QuestionPart_QuestionPartOrderWithinQuestion,
            [qps].[QuestionPartType] AS QuestionPart_QuestionPartType,

            [qp].[Id] AS QuestionPartPrompt_QuestionPartId,
			[qp].[QuestionText] AS QuestionPartPrompt_QuestionText,
			[qp].[HintText] AS QuestionPartPrompt_HintText,

			[qp].[Id] AS QuestionPartMultipleAnswerItemControl_QuestionPartId,
			[qp].[AllowMultipleAnswerItems] AS QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed,
			[qmpari].[ItemDescription] AS QuestionPartMultipleAnswerItemControl_ItemDescription,
			[qmpari].[CollectionDescription] AS QuestionPartMultipleAnswerItemControl_CollectionDescription,

			[qp].[Id] AS QuestionPartResponseTypeInformation_QuestionPartId,
			[qp].[ResponseFormatType] AS QuestionPartResponseTypeInformation_FormatType,
			[rft].[InputType] AS QuestionPartResponseTypeInformation_InputType,

			[ap].[Id] AS QuestionPartAnswer_Id,
            [ap].[QuestionPart] AS QuestionPartAnswer_QuestionPartId,

			[apr].[Id] AS QuestionPartAnswerResponse_Id,
			[apr].[OrderWithinAnswerPart] AS QuestionPartAnswerItem_OrderWithinAnswerPart,
			[rft].[InputType] AS QuestionPartAnswerItem_InputType

        FROM [dbo].[DataShareRequest] dsr
            JOIN [dbo].[QuestionSet] qs ON [qs].[Id] = [dsr].[QuestionSet]
			JOIN [dbo].[QuestionSetSection] qss ON [qss].[QuestionSet] = [qs].[Id]
			JOIN [dbo].[QuestionSetQuestion] qsq ON [qsq].[QuestionSetSection] = [qss].[Id]
			JOIN [dbo].[Question] q ON [q].[Id] = [qsq].[Question]
			JOIN [dbo].[QuestionPartSet] qps ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartType] = 'MainQuestionPart'
			JOIN [dbo].[QuestionPart] qp ON [qp].[Id] = [qps].[QuestionPart]
			JOIN [dbo].[ResponseFormatType] rft ON [rft].[Id] = [qp].[ResponseFormatType]
			LEFT JOIN [dbo].[QuestionPartMultiAnswerResponseInformation] qmpari ON [qmpari].[QuestionPart] = [qp].[Id]

			JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
			JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
			JOIN [dbo].[AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]
			LEFT JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]

        WHERE   ([dsr].[Id] = @DataShareRequestId)
			AND ([q].[Id] = @QuestionId)
        ORDER BY [qps].[QuestionPartOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionFooterData =>
      @"SELECT
	        [qf].[Id] AS DataShareRequestQuestionFooter_Id,
	        [qf].[Header] AS DataShareRequestQuestionFooter_Header,

	        [qfi].[Id] AS DataShareRequestQuestionFooterItem_Id,
	        [qfi].[QuestionFooter] AS DataShareRequestQuestionFooterItem_FooterId,
	        [qfi].[Text] AS DataShareRequestQuestionFooterItem_Text,
	        [qfi].[OrderWithinFooter] AS DataShareRequestQuestionFooterItem_OrderWithinFooter

        FROM [dbo].[QuestionFooter] [qf]
	        LEFT JOIN [dbo].[QuestionFooterItem] [qfi] ON [qfi].[QuestionFooter] = [qf].[Id]
        WHERE [qf].[Question] = @QuestionId
        ORDER BY
	        [qfi].[OrderWithinFooter]";

    string IAcquirerDataShareRequestSqlQueries.GetFreeFormOptionsModelData =>
      @"SELECT
	        [ffo].[Id] AS QuestionPartResponseFormatFreeFormOptions_Id,
	        [ffo].[QuestionPart] AS QuestionPartResponseFormatFreeFormOptions_QuestionPartId,
	        [ffo].[ValueEntryMayBeDeclined] AS QuestionPartResponseFormatFreeFormOptions_ValueEntryMayBeDeclined
        FROM [dbo].[FreeFormOptions] ffo
        WHERE [ffo].[QuestionPart] = @QuestionPartId";

    string IAcquirerDataShareRequestSqlQueries.GetSelectionOptionSingleValueModelData =>
      @"SELECT 
	        [so].[Id] AS OptionSelectionItem_Id,
	        [so].[OptionValueText] AS OptionSelectionItem_ValueText,
	        [so].[OptionHintText] AS OptionSelectionItem_HintText,
	        [so].[OptionOrder] AS OptionSelectionItem_OptionOrderWithinSelection,
            [so].[SupplementaryQuestionPart] AS OptionSelectionItem_SupplementaryQuestionPartId,
	        [sosv].[IsAlternativeAnswer] AS SingleSelectionOption_IsAlternativeAnswer
        FROM [dbo].[SelectionOptionSingleValue] sosv
	        JOIN [dbo].[SelectionOption] [so] ON [so].Id = [sosv].[Id]
        WHERE [so].[QuestionPart] = @QuestionPartId
        ORDER BY
	        [so].[OptionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetSelectionOptionMultiValueModelData =>
      @"SELECT 
	        [so].[Id] AS OptionSelectionItem_Id,
	        [so].[OptionValueText] AS OptionSelectionItem_ValueText,
	        [so].[OptionHintText] AS OptionSelectionItem_HintText,
	        [so].[OptionOrder] AS OptionSelectionItem_OptionOrderWithinSelection,
            [so].[SupplementaryQuestionPart] AS OptionSelectionItem_SupplementaryQuestionPartId,
	        [somv].[IsMaster] AS MultiSelectOption_IsMaster
        FROM [dbo].[SelectionOptionMultiValue] somv
	        JOIN [dbo].[SelectionOption] [so] ON [so].Id = [somv].[Id]
        WHERE [so].[QuestionPart] = @QuestionPartId
        ORDER BY
	        [so].[OptionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetSupplementaryQuestionPartModelData =>
      @"SELECT
            [qp].[Id] AS QuestionPart_Id,

            'SupplementaryQuestionPart' AS QuestionPart_QuestionPartType,

	        [qp].[Id] AS QuestionPartPrompt_QuestionPartId,
	        [qp].[QuestionText] AS QuestionPartPrompt_QuestionText,
	        [qp].[HintText] AS QuestionPartPrompt_HintText,

            [qp].[Id] AS QuestionPartMultipleAnswerItemControl_QuestionPartId,
			[qp].[AllowMultipleAnswerItems] AS QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed,
			[qmpari].[ItemDescription] AS QuestionPartMultipleAnswerItemControl_ItemDescription,
			[qmpari].[CollectionDescription] AS QuestionPartMultipleAnswerItemControl_CollectionDescription,

	        [qp].[Id] AS QuestionPartResponseTypeInformation_QuestionPartId,
	        [qp].[ResponseFormatType] AS QuestionPartResponseTypeInformation_FormatType,
	        [rft].[InputType] AS QuestionPartResponseTypeInformation_InputType
        FROM [dbo].[QuestionPart] qp
	        JOIN [dbo].[ResponseFormatType] rft ON [rft].[Id] = [qp].[ResponseFormatType]
            LEFT JOIN [dbo].[QuestionPartMultiAnswerResponseInformation] qmpari ON [qmpari].[QuestionPart] = [qp].[Id]
        WHERE [qp].[Id] = @QuestionPartId";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionPartAnswerResponseItemFreeFormModelData =>
      @"SELECT
	        [apri].[Id] AS QuestionPartAnswerItem_Id,
	        [apr].[OrderWithinAnswerPart] AS QuestionPartAnswerItem_OrderWithinAnswerPart,
	        [apriff].[EnteredValue] AS QuestionPartAnswerItemFreeForm_EnteredValue,
	        [apriff].[ValueEntryDeclined] AS QuestionPartAnswerItemFreeForm_ValueEntryDeclined
        FROM [dbo].[AnswerPartResponseItem] [apri]
	        JOIN [dbo].[AnswerPartResponseItemFreeForm] [apriff] ON [apriff].[Id] = [apri].[Id]
	        JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[Id] = [apri].[AnswerPartResponse]
        WHERE [apriff].[Id] = @AnswerPartResponseItemId";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionPartAnswerResponseItemOptionSelectionModelData =>
      @"SELECT
            [apri].[Id] AS QuestionPartAnswerItem_Id,
            [apr].[OrderWithinAnswerPart] AS QuestionPartAnswerItem_OrderWithinAnswerPart,

            [apriso].[SelectionOption] AS QuestionPartAnswerItem_OptionSelectionItemId,
            [apriso].[SupplementaryQuestionPartAnswer] AS QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId

        FROM [dbo].[AnswerPartResponseItem] [apri]
	        JOIN [dbo].[AnswerPartResponseItemSelectionOption] [apriso] ON [apriso].[AnswerPartResponseItem] = [apri].[Id]
	        JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[Id] = [apri].[AnswerPartResponse]
	        JOIN [dbo].[SelectionOption] [so] ON [so].[Id] = [apriso].[SelectionOption]
        WHERE [apri].[Id] = @AnswerPartResponseItemId
        ORDER BY
	        [so].[OptionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetSupplementaryQuestionAnswerPartAnswerModelData =>
      @"SELECT
	        [ap].[Id] AS QuestionPartAnswer_Id,
	        [ap].[QuestionPart] AS QuestionPartAnswer_QuestionPartId,

	        [apr].[Id] AS QuestionPartAnswerResponse_Id,
	        [apr].[OrderWithinAnswerPart] AS QuestionPartAnswerItem_OrderWithinAnswerPart,
	        [rft].[InputType] AS QuestionPartAnswerItem_InputType

        FROM [dbo].[AnswerPart] [ap]
	        JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [ap].[QuestionPart]
	        JOIN [dbo].[ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType]
        WHERE [ap].[Id] = @SupplementaryQuestionPartAnswerId";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionStatusInformationsModelData =>
      @"SELECT
            [dsr].[Id] AS DataShareRequestId,

            [q].[Id] AS DataShareRequestQuestionStatus_QuestionId,

            [qss].[SectionNumber] AS QuestionSet_SectionNumber,
	        [qsq].[QuestionOrder] AS QuestionSet_QuestionOrerWithinSection,

	        [q].[Id] AS QuestionResponseInformation_QuestionId,
	        [a].[QuestionStatus] AS QuestionResponseInformation_QuestionStatusType,
			            
            [rft].[InputType] AS QuestionPartResponse_ResponseInputType,
            [apr].[Id] AS QuestionPartResponse_AnswerPartResponseId,

	        [qsp].[Question] AS QuestionPreRequisite_QuestionId,
	        [qsp].[PreRequisiteQuestion] AS QuestionPreRequisite_PreRequisiteQuestionId,

	        [qssoqro].[ControlledQuestionSetQuestion] AS QuestionSetQuestionApplicabilityOverride_ControlledQuestionId,
	        [qssoqro].[ControllingSelectionOption] AS QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionId,
	        [qssoqro].[ControlledQuestionApplicabilityCondition] AS QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition,
	        [controlling_apriso].[SelectionOption] AS SelectedOptionId,
	        CAST (CASE WHEN [controlling_apriso].[SelectionOption] IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected

        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSetSection] = [qss].[Id]
	        JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartType] = 'MainQuestionPart'
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].Id = [qps].[QuestionPart]
	        JOIN [dbo].[ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType]

	        JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
	        JOIN [dbo].[AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id] AND [apr].[OrderWithinAnswerPart] = 1 -- is there at least one response to the question part

	        LEFT JOIN [dbo].[QuestionSetPreRequisite] [qsp] ON [qsp].[QuestionSet] = [qs].[Id] AND [qsp].[Question] = [q].[Id]

	        LEFT JOIN [dbo].[QuestionSetSelectionOptionQuestionApplicabilityOverride] [qssoqro] ON [qssoqro].[ControlledQuestionSetQuestion] = [qsq].[Id]
	        LEFT JOIN [dbo].[SelectionOption] [so] ON [so].[Id] = [qssoqro].[ControllingSelectionOption]

	        LEFT JOIN [dbo].[QuestionPart] [controlling_qp] ON [controlling_qp].[Id] = [so].[QuestionPart]
	        LEFT JOIN [dbo].[QuestionPartSet] [controlling_qps] ON [controlling_qps].[QuestionPart] = [controlling_qp].[Id] AND [controlling_qps].[QuestionPartType] = 'MainQuestionPart'
	        LEFT JOIN [dbo].[Question] [controlling_q] ON [controlling_q].[Id] = [controlling_qps].[Question]
	        LEFT JOIN [dbo].[Answer] [controlling_a] ON [controlling_a].[Question] = [controlling_q].[Id] AND [controlling_a].[AnswerSet] = [as].[Id]
	        LEFT JOIN [dbo].[AnswerPart] [controlling_ap] ON [controlling_ap].[Answer] = [controlling_a].[Id] AND [controlling_ap].[QuestionPart] = [controlling_qp].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponse] [controlling_apr] ON [controlling_apr].[AnswerPart] = [controlling_ap].[Id] AND [controlling_apr].[OrderWithinAnswerPart] = 1 -- is there at least one response to the question part
		    LEFT JOIN [dbo].[AnswerPartResponseItem] [controlling_apri] ON [controlling_apri].[AnswerPartResponse] = [controlling_apr].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemSelectionOption] [controlling_apriso] ON [controlling_apriso].[AnswerPartResponseItem] = [controlling_apri].[Id] And [qssoqro].[ControllingSelectionOption] = [controlling_apriso].[SelectionOption]
        WHERE [dsr].[Id] = @DataShareRequestId
        ORDER BY
	        [qss].[SectionNumber],
            [qsq].[QuestionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionGroups =>
      @"SELECT
	        [dsr].[Id] AS DataShareRequestAnswersSummary_DataShareRequestId,
            [dsr].[EsdaName] AS DataShareRequestAnswersSummary_EsdaName,
	        [dsr].[RequestId] AS DataShareRequestAnswersSummary_RequestId,
	        [dsr].[RequestStatus] AS DataShareRequestAnswersSummary_RequestStatus,
	        [dsr].[QuestionsRemainThatRequireAResponse] AS DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse,

	        [qss].[Id] AS DataShareRequestAnswersSummarySection_SectionId,
	        [qss].[SectionNumber] AS DataShareRequestAnswersSummarySection_OrderWithinSummary,
	        [qss].[SectionHeader] AS DataShareRequestAnswersSummarySection_SectionHeader,

	        [qsqmain].[Question] AS DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId,
	        [qsqmain].[QuestionOrder] AS DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection,
	        [qsqbacking].[Question] AS DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionId

        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]

	        JOIN [dbo].[QuestionSetQuestion] [qsqmain] ON [qsqmain].[QuestionSetSection] = [qss].[Id] AND [qsqmain].[FrontingQuestion] IS NULL
	        JOIN [dbo].[Question] [qmain] ON [qsqmain].[Question] = [qmain].[Id]
            JOIN [dbo].[Answer] [amain] ON [amain].[AnswerSet] = [as].[Id] AND [amain].[Question] = [qmain].[Id]
                AND [amain].[QuestionStatus] != 'NotApplicable'
                AND [amain].[QuestionStatus] != 'NoResponseNeeded'

	        LEFT JOIN [dbo].[QuestionSetQuestion] [qsqbacking] ON [qsqbacking].[FrontingQuestion] = [qmain].[Id]
	        LEFT JOIN [dbo].[Question] [qbacking] ON [qbacking].[Id] = [qsqbacking].[Question]
	        
        WHERE [dsr].[Id] = @DataShareRequestId
        ORDER BY
	        [qss].[SectionNumber],
	        [qsqmain].[QuestionOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionAnswer =>
      @"SELECT
	        [q].[Id] AS DataShareRequestAnswersSummaryQuestion_QuestionId,
	        [q].[Header] AS DataShareRequestAnswersSummaryQuestion_QuestionHeader,
	        CAST (CASE WHEN [a].[QuestionStatus] = 'NotApplicable' THEN 0 ELSE 1 END AS BIT) AS DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable,
	        [qp].[Id] AS DataShareRequestAnswersSummaryQuestion_QuestionPartId

        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [dbo].[QuestionSetSection] [qss] ON [qss].[QuestionSet] = [qs].[Id]
	        JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSetSection] = [qss].[Id]
	        JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartType] = 'MainQuestionPart'
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]

	        JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
	        JOIN [dbo].[AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]
	        
        WHERE [dsr].[Id] = @DataShareRequestId
	        AND [q].[Id] = @QuestionId
        ORDER BY
	        [qss].[SectionNumber],
	        [qsq].[QuestionOrder],
	        [qps].[QuestionPartOrder]";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds =>
      @"SELECT
	        [qp].[Id] AS DataShareRequestAnswersSummaryQuestionPart_QuestionPartId,
	        [ap].[Id] AS DataShareRequestAnswersSummaryQuestionPart_AnswerPartId,
	        [qps].[QuestionPartOrder] AS DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion,
            [qp].[QuestionText] AS DataShareRequestAnswersSummaryQuestionPart_QuestionPartText,
	        [qp].[AllowMultipleAnswerItems] AS DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed,
	        [qpmari].[CollectionDescription] AS DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed,
	        [rft].[InputType] AS DataShareRequestAnswersSummaryQuestionPart_ResponseInputType,
            [rft].[Id] AS DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType,
	        [apr].[Id] AS DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId,
	        [apr].[OrderWithinAnswerPart] AS DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart

        FROM [dbo].[DataShareRequest] [dsr]
	        JOIN [QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	        JOIN [QuestionSetQuestion] [qsq] ON [qsq].[QuestionSet] = [qs].[Id]
	        JOIN [Question] [q] ON [q].[Id] = [qsq].[Question]
	        JOIN [QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id]
	        JOIN [QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]
	        JOIN [ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType]
	        JOIN [AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	        JOIN [Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
	        JOIN [AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]

	        LEFT JOIN [QuestionPartMultiAnswerResponseInformation] [qpmari] ON [qpmari].[QuestionPart] = [qp].[Id]
	        LEFT JOIN [AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]
        WHERE	[dsr].[Id] = @DataShareRequestId
	         AND [qp].[Id] = @QuestionPartId
        ORDER BY [apr].[OrderWithinAnswerPart]";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerResponseFreeFormItems =>
      @"SELECT
            [apri].[Id] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseItemId,
	        [apriff].[EnteredValue] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_AnswerValue,
	        [apriff].[ValueEntryDeclined] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_ValueEntryDeclined
        FROM [dbo].[AnswerPartResponse] [apr]
	        LEFT JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemFreeForm] [apriff] ON [apriff].[Id] = [apri].[Id]
        WHERE [apr].[Id] = @AnswerPartResponseId";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerResponseOptionSelectionItems =>
        @"SELECT
            [apri].[Id] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseItemId,
	        [apriso].[Id] AS DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_ItemId,
	        [so].[OptionOrder] AS DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_OrderWithinAnswerPartResponse,
	        [so].[OptionValueText] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SelectionOptionText,
	        [supp_apriff].[EnteredValue] AS DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SupplementaryAnswerText
        FROM [dbo].[AnswerPartResponse] [apr]
	        LEFT JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItemSelectionOption] [apriso] ON [apriso].[AnswerPartResponseItem] = [apri].[Id]
	        LEFT JOIN [dbo].[SelectionOption] [so] ON [so].[Id] = [apriso].[SelectionOption]

	        LEFT JOIN [dbo].[AnswerPart] [supp_ap] ON [supp_ap].[Id] = [apriso].[SupplementaryQuestionPartAnswer]
	        LEFT JOIN [dbo].[AnswerPartResponse] [supp_apr] ON [supp_apr].[AnswerPart] = [supp_ap].[Id]
	        LEFT JOIN [dbo].[AnswerPartResponseItem] [supp_apri] ON [supp_apri].[AnswerPartResponse] = [supp_apr].[Id]
	        -- We cheat here and assume that supplementary answers, if given, will always be of free form response type
	        LEFT JOIN [dbo].[AnswerPartResponseItemFreeForm] [supp_apriff] ON [supp_apriff].[Id] = [supp_apri].[Id]
        WHERE [apr].[Id] = @AnswerPartResponseId
        ORDER BY [so].[OptionOrder]";

    string IAcquirerDataShareRequestSqlQueries.SubmitDataShareRequest =>
      @"UPDATE [dbo].[DataShareRequest]
        SET [RequestStatus] = 'Submitted'
        WHERE [Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.GetSubmissionIdsForDataShareRequest =>
      @"SELECT [s].[Id]
        FROM [dbo].[Submission] [s]
        WHERE [s].[DataShareRequest] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.CreateSubmissionForDataShareRequest =>
      @"INSERT INTO [dbo].[Submission] (DataShareRequest)
        OUTPUT [inserted].[Id]
        VALUES (@DataShareRequestId)";

    string IAcquirerDataShareRequestSqlQueries.UpdateDataShareRequestCompleteness =>
      @"UPDATE [dbo].[DataShareRequest]
        SET	[QuestionsRemainThatRequireAResponse] = @QuestionsRemainThatRequireAResponse
        WHERE [Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestNotificationInformation =>
      @"SELECT
            [dsr].[SupplierOrganisation] AS SupplierOrganisationId,
            [dsr].[SupplierDomain] AS SupplierDomainId,
	        [dsr].[AcquirerUser] AS AcquirerUserId,
            [dsr].[RequestId] AS DataShareRequestRequestId,
            [dsr].[Esda] AS EsdaId,
	        [dsr].[EsdaName] AS EsdaName
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.GetWhetherQuestionsRemainThatRequireAResponse =>
      @"SELECT [dsr].[QuestionsRemainThatRequireAResponse]
        FROM [dbo].[DataShareRequest] [dsr]
        WHERE [dsr].[Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.CancelDataShareRequest =>
      @"UPDATE [dbo].[DataShareRequest]
        SET [RequestStatus] = 'Cancelled'
        WHERE [Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.DeleteDataShareRequest =>
        @"UPDATE [dbo].[DataShareRequest]
        SET [RequestStatus] = 'Deleted'
        WHERE [Id] = @DataShareRequestId";

    string IAcquirerDataShareRequestSqlQueries.CheckIfDataShareRequestExists =>
        @"SELECT CASE WHEN EXISTS (
            SELECT 1 FROM [dbo].[DataShareRequest] [dsr]
            WHERE [dsr].[Id] = @DataShareRequestId)
        THEN 1 ELSE 0 END";

    #region Saving Answer Data
    string IAcquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionAnswerId =>
      @"SELECT [a].[Id]
        FROM [dbo].[AnswerSet] [as]
	        JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id]
        WHERE [as].[DataShareRequest] = @DataShareRequestId
	        AND [a].[Question] = @QuestionId";

    string IAcquirerDataShareRequestSqlQueries.GetQuestionAnswerPartId =>
      @"SELECT [ap].Id
        FROM [dbo].[AnswerPart] [ap]
        WHERE   [ap].[Answer] = @AnswerId
	        AND [ap].[QuestionPart] = @QuestionPartId";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerPartResponseIds =>
      @"SELECT [apr].[Id]
        FROM [dbo].[AnswerPartResponse] [apr]
        WHERE [apr].[AnswerPart] = @AnswerPartId";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemId =>
      @"SELECT [apri].[Id]
        FROM [dbo].[AnswerPartResponseItem] [apri]
        WHERE [AnswerPartResponse] = @AnswerPartResponseId";

    string IAcquirerDataShareRequestSqlQueries.DeleteAnswerPartResponse =>
        @"DELETE FROM [dbo].[AnswerPartResponse]
          WHERE [Id] = @AnswerPartResponseId";

    string IAcquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItem =>
        @"DELETE FROM [dbo].[AnswerPartResponseItem]
          WHERE [Id] = @AnswerPartResponseItemId";

    string IAcquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItemFreeForm =>
    @"DELETE FROM [dbo].[AnswerPartResponseItemFreeForm]
        WHERE [Id] = @AnswerPartResponseItemId";

    string IAcquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItemOptionSelect =>
      @"DELETE FROM [dbo].[AnswerPartResponseItemSelectionOption]
        WHERE [Id] = @Id";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerPartResponse =>
      @"INSERT INTO [dbo].[AnswerPartResponse] (AnswerPart, OrderWithinAnswerPart)
        OUTPUT [inserted].[Id]
        VALUES (
	        @AnswerPartId,
	        @OrderWithinAnswerPart)";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItem =>
      @"INSERT INTO [dbo].[AnswerPartResponseItem] (AnswerPartResponse)
        OUTPUT [inserted].[Id]
        VALUES (
            @AnswerPartResponseId)";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItemFreeForm =>
      @"INSERT INTO [dbo].[AnswerPartResponseItemFreeForm] (Id, EnteredValue, ValueEntryDeclined)
        VALUES (
	        @Id,
	        @EnteredValue,
	        @ValueEntryDeclined)";

    string IAcquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItemSelectionOption =>
      @"INSERT INTO [dbo].[AnswerPartResponseItemSelectionOption] (AnswerPartResponseItem, SelectionOption, SupplementaryQuestionPartAnswer)
        VALUES (
	        @AnswerPartResponseItemId,
	        @SelectionOptionId,
	        @SupplementaryQuestionPartAnswerId)";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemSelectionOptionIds =>
      @"SELECT [Id]
        FROM [dbo].[AnswerPartResponseItemSelectionOption]
        WHERE [AnswerPartResponseItem] = @AnswerPartResponseItemId";

    string IAcquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemSelectionOptionSupplementaryAnswerPartModelData =>
      @"SELECT
	        [ap].[Id] AS SupplementaryAnswerPart_AnswerPartId,
	        [rft].[InputType] AS SupplementaryAnswerPart_InputType
        FROM [dbo].[AnswerPartResponseItemSelectionOption] [apriso]
	        JOIN [dbo].[AnswerPart] [ap] ON [ap].[Id] = [apriso].[SupplementaryQuestionPartAnswer]
	        JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [ap].[QuestionPart]
	        JOIN [dbo].[ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType]
        WHERE [apriso].[Id] = @Id";
    #endregion
}