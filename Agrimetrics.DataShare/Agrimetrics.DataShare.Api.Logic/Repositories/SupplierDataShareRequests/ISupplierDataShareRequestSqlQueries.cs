namespace Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;

public interface ISupplierDataShareRequestSqlQueries
{
    string GetPendingSubmissionSummaries { get; }

    string GetCompletedSubmissionSummaries { get; }

    string GetSubmissionInformationModelData { get; }

    string GetReturnedSubmissionInformation { get; }

    string GetCompletedSubmissionInformation { get; }

    string GetSubmissionNotes { get; }

    string SetSubmissionNotes { get; }

    string GetDataShareRequestStatus { get; }

    string UpdateDataShareRequestStatus { get; }

    string GetAcceptedDecisionSummary { get; }

    string GetRejectedDecisionSummary { get; }

    string GetReturnedDecisionSummary { get; }
    
    string GetSubmissionDetailsModelDatas { get; }

    string GetSubmissionDetailsAnswerPartResponseModelDatas { get; }

    string GetDataShareRequestNotificationInformation { get; }
}