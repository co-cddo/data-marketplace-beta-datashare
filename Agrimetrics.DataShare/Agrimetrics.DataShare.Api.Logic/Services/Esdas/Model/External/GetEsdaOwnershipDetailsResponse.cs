namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;

public class GetEsdaOwnershipDetailsResponse
{
    public required Guid EsdaId { get; set; }

    public required string Title { get; set; }

    public required int OrganisationId { get; set; }

    public required int DomainId { get; set; }

    public string? ContactPointName { get; set; }

    public string? ContactPointEmailAddress { get; set; }

    public DataShareRequestNotificationRecipientType? DataShareRequestNotificationRecipientType { get; set; }

    public string? CustomDsrNotificationAddress { get; set; }
}