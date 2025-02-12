using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;

internal class EsdaDetails : IEsdaDetails
{
    public required Guid Id { get; init; }
    
    public required string Title { get; init; }

    public required int SupplierOrganisationId { get; init; }

    public required int SupplierDomainId { get; init; }

    public required string? ContactPointName { get; init; }
    
    public required string? ContactPointEmailAddress { get; init; }

    public required DataShareRequestNotificationRecipientType? DataShareRequestNotificationRecipientType { get; init; }

    public required string? CustomDsrNotificationAddress { get; init; }
}