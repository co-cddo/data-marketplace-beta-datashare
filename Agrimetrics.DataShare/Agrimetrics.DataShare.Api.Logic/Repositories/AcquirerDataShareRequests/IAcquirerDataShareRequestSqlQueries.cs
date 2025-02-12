namespace Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;

public interface IAcquirerDataShareRequestSqlQueries
{
    string GetMasterQuestionSetId { get; }

    string GetQuestionSetIdForEsdaAndSupplierDomain { get; }

    string GetQuestionSetIdForEsdaAndSupplierOrganisation { get; }

    string GetDefaultQuestionSetIdForSupplierDomain { get; }

    string GetDefaultQuestionSetIdForSupplierOrganisation { get; }

    string GetQuestionSetOutlineRequest { get; }

    string GetEsdaNameForDataShareRequest { get; }

    string GetTotalNumberOfDataShareRequests { get; }

    string CreateDataShareRequest { get; }

    string GetDataShareRequestRequestId { get; }

    string CreateAnswerSet { get; }

    string CreateAnswer { get; }

    string CreateAnswerPart { get; }

    string GetDataShareRequestStatus { get; }

    string GetDataShareRequestAnswerSetId { get; }

    string UpdateDataShareRequestQuestionStatus { get; }

    string GetQuestionSetQuestionParts { get; }

    string GetDataShareRequestModelData { get; }

    string GetDataShareRequestQuestionsSummaryModelData { get; }

    string GetDataShareRequestQuestionModelData { get; }

    string GetDataShareRequestQuestionFooterData { get; }

    string GetFreeFormOptionsModelData { get; }

    string GetSelectionOptionSingleValueModelData { get; }

    string GetSelectionOptionMultiValueModelData { get; }

    string GetSupplementaryQuestionPartModelData { get; }

    string GetQuestionPartAnswerResponseItemFreeFormModelData { get; }

    string GetQuestionPartAnswerResponseItemOptionSelectionModelData { get; }

    string GetSupplementaryQuestionAnswerPartAnswerModelData { get; }

    string GetDataShareRequestQuestionStatusInformationsModelData { get; }

    string GetDataShareRequestAnswersSummaryQuestionGroups { get; }

    string GetDataShareRequestAnswersSummaryQuestionAnswer { get; }

    string GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds { get; }

    string GetAnswerResponseFreeFormItems { get; }

    string GetAnswerResponseOptionSelectionItems { get; }

    string SubmitDataShareRequest { get; }

    string GetSubmissionIdsForDataShareRequest { get; }

    string CreateSubmissionForDataShareRequest { get; }

    string UpdateDataShareRequestCompleteness { get; }

    string GetDataShareRequestNotificationInformation { get; }

    string GetWhetherQuestionsRemainThatRequireAResponse { get; }

    string CancelDataShareRequest { get; }

    string DeleteDataShareRequest { get; }

    string CheckIfDataShareRequestExists { get; }

    #region Saving Answer Data
    string GetDataShareRequestQuestionAnswerId { get; }
    
    string GetQuestionAnswerPartId { get; }
    
    string GetAnswerPartResponseIds { get; }

    string GetAnswerPartResponseItemId { get; }

    string GetAnswerPartResponseItemSelectionOptionIds { get; }

    string DeleteAnswerPartResponse { get; }

    string DeleteAnswerPartResponseItem { get; }

    string DeleteAnswerPartResponseItemFreeForm { get; }
    
    string DeleteAnswerPartResponseItemOptionSelect { get; }
    
    string CreateAnswerPartResponse { get; }

    string CreateAnswerPartResponseItem { get; }

    string CreateAnswerPartResponseItemFreeForm { get; }

    string CreateAnswerPartResponseItemSelectionOption { get; }

    string GetAnswerPartResponseItemSelectionOptionSupplementaryAnswerPartModelData { get; }
    #endregion
}