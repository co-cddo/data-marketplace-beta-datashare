using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

public interface INotificationsConfigurationPresenter
{
    string GetGovNotifyApiKey();

    Guid GetSupplierNewDataShareRequestReceivedTemplateId();

    Guid GetSupplierDataShareRequestCancelledTemplateId();

    Guid GetAcquirerDataShareRequestAcceptedTemplateId();

    Guid GetAcquirerDataShareRequestRejectedTemplateId();

    Guid GetAcquirerDataShareRequestReturnedWithCommentsTemplateId();

    string GetDataShareRequestNotificationCddoAdminEmailAddress();

    string GetDataShareRequestNotificationCddoAdminName();

    IEnumerable<SettingValue> GetAllSettingValues();
}