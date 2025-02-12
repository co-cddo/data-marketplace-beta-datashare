namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class DataShareRequestForResourceForAcquirerOrganisationSummaryModelData
{
    public Guid DataShareRequestForResourceForAcquirerOrganisationSummary_RequestId { get; set; }

    public int DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerUserId { get; set; }

    public int DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerDomainId { get; set; }

    public int DataShareRequestForResourceForAcquirerOrganisationSummary_AcquirerOrganisationId { get; set; }

    public int DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierDomainId { get; set; }

    public int DataShareRequestForResourceForAcquirerOrganisationSummary_SupplierOrganisationId { get; set; }

    public Guid DataShareRequestForResourceForAcquirerOrganisationSummary_EsdaId { get; set; }

    public string DataShareRequestForResourceForAcquirerOrganisationSummary_RequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType DataShareRequestForResourceForAcquirerOrganisationSummary_RequestStatus { get; set; }

    public DateTime WhenCreatedLocal { get; set; }

    public DateTime? WhenSubmittedLocal { get; set; }

    public DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData DataShareRequestForResourceForAcquirerOrganisationSummary_OwnerContactDetails { get; set; }
}