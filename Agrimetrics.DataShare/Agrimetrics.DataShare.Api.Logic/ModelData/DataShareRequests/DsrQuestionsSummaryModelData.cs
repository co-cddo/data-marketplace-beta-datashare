using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class DataShareRequestQuestionsSummaryModelData
{
    public Guid DataShareRequest_Id { get; set; }

    public string DataShareRequest_RequestId { get; set; } = string.Empty;

    public string DataShareRequest_EsdaName { get; set; } = string.Empty;

    public int DataShareRequest_AcquirerUserId { get; set; }

    public int DataShareRequest_AcquirerDomainId { get; set; }

    public int DataShareRequest_AcquirerOrganisationId { get; set; }

    public int DataShareRequest_SupplierOrganisationId { get; set; }

    public QuestionSetSummaryModelData DataShareRequest_QuestionSetSummary { get; set; }

    public DataShareRequestStatusType DataShareRequest_DataShareRequestStatus { get; set; }

    public bool DataShareRequest_QuestionsRemainThatRequireAResponse { get; set; }

    public string? DataShareRequest_SupplierOrganisationName { get; set; }

    public string? DataShareRequest_SubmissionResponseFromSupplier { get; set; }

    public string? DataShareRequest_CancellationReasonsFromAcquirer { get; set; }
}