namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests
{
    public class DataShareRequestNotificationInformationModelData
    {
        public int SupplierOrganisationId { get; set; }

        public int SupplierDomainId { get; set; }

        public int AcquirerUserId { get; set; }

        public string DataShareRequestRequestId { get; set; } = string.Empty;
        
        public Guid EsdaId { get; set; }

        public string EsdaName { get; set; } = string.Empty;
    }
}
