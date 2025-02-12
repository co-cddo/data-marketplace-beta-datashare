using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

public interface ISupplierNewDataShareRequestReceivedNotification : IEmailNotification
{
    string SupplierOrganisationEmailAddress { get; }

    string SupplierOrganisationName { get; }

    string AcquirerOrganisationName { get; }

    string EsdaName { get; }

    string DataMarketPlaceSignInAddress { get; }
}