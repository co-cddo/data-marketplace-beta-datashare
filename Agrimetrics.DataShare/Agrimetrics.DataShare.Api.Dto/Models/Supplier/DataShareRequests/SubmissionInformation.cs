using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class SubmissionInformation
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatus RequestStatus { get; set; }

    public string EsdaName { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public List<string> DataTypes { get; set; } = [];

    public string ProjectAims { get; set; } = string.Empty;

    public DateTime? WhenNeededBy { get; set; }

    public DateTime SubmittedOn { get; set; }

    public string AcquirerEmailAddress { get; set; } = string.Empty;

    public List<string> AnswerHighlights { get; set; } = [];
}