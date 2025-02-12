using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class ReturnedSubmissionInformation
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatus RequestStatus { get; set; }

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public string EsdaName { get; set; } = string.Empty;

    public DateTime SubmittedOn { get; set; }

    public DateTime ReturnedOn { get; set; }

    public DateTime? WhenNeededBy { get; set; }

    public string SupplierNotes { get; set; } = string.Empty;

    public string FeedbackProvided { get; set; } = string.Empty;
}