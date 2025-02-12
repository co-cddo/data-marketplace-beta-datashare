using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class SubmissionDetails
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatus RequestStatus { get; set; }

    public string EsdaName { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public List<SubmissionDetailsSection> Sections { get; set; } = [];

    public SubmissionReturnDetailsSet SubmissionReturnDetailsSet { get; set; }
}