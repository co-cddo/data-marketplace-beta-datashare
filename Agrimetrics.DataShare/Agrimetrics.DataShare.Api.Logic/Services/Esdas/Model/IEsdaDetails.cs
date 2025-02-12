using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model
{
    public interface IEsdaDetails
    {
        Guid Id { get; }

        string Title { get; }

        int SupplierOrganisationId { get; }

        int SupplierDomainId { get; }

        string? ContactPointName { get; }

        string? ContactPointEmailAddress { get; }

        DataShareRequestNotificationRecipientType? DataShareRequestNotificationRecipientType { get; }

        string? CustomDsrNotificationAddress { get; }
    }
}
