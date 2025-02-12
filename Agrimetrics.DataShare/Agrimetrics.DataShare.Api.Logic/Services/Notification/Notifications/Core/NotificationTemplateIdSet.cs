using Agrimetrics.DataShare.Api.Logic.Configuration;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

internal class NotificationTemplateIdSet(
    INotificationsConfigurationPresenter notificationsConfigurationPresenter) : INotificationTemplateIdSet
{
    public Guid SupplierNewDataShareRequestReceivedId => notificationsConfigurationPresenter.GetSupplierNewDataShareRequestReceivedTemplateId();

    public Guid SupplierDataShareRequestCancelledId => notificationsConfigurationPresenter.GetSupplierDataShareRequestCancelledTemplateId();

    public Guid AcquirerDataShareRequestAcceptedId => notificationsConfigurationPresenter.GetAcquirerDataShareRequestAcceptedTemplateId();

    public Guid AcquirerDataShareRequestRejectedId => notificationsConfigurationPresenter.GetAcquirerDataShareRequestRejectedTemplateId();

    public Guid AcquirerDataShareRequestReturnedWithCommentsId => notificationsConfigurationPresenter.GetAcquirerDataShareRequestReturnedWithCommentsTemplateId();
}