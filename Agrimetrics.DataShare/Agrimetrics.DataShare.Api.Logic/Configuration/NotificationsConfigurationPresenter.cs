using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

internal class NotificationsConfigurationPresenter(
    IServiceConfigurationPresenter serviceConfigurationPresenter) : INotificationsConfigurationPresenter
{
    private const string notificationsSectionName = "Notifications";
    private const string templatesSectionName = "TemplateIds";

    string INotificationsConfigurationPresenter.GetGovNotifyApiKey()
    {
        return DoGetGovNotifyApiKey();
    }

    Guid INotificationsConfigurationPresenter.GetSupplierNewDataShareRequestReceivedTemplateId()
    {
        var newDataShareRequestReceivedTemplateId =  DoGetSupplierNewDataShareRequestReceivedTemplateId();
        return Guid.Parse(newDataShareRequestReceivedTemplateId);
    }

    Guid INotificationsConfigurationPresenter.GetSupplierDataShareRequestCancelledTemplateId()
    {
        var supplierDataShareRequestCancelledTemplateId = DoGetSupplierDataShareRequestCancelledTemplateId();

        return Guid.Parse(supplierDataShareRequestCancelledTemplateId);
    }

    Guid INotificationsConfigurationPresenter.GetAcquirerDataShareRequestAcceptedTemplateId()
    {
        var acquirerDataShareRequestAcceptedTemplateId = DoGetAcquirerDataShareRequestAcceptedTemplateId();

        return Guid.Parse(acquirerDataShareRequestAcceptedTemplateId);
    }

    Guid INotificationsConfigurationPresenter.GetAcquirerDataShareRequestRejectedTemplateId()
    {
        var acquirerDataShareRequestRejectedTemplateId = DoGetAcquirerDataShareRequestRejectedTemplateId();

        return Guid.Parse(acquirerDataShareRequestRejectedTemplateId);
    }

    Guid INotificationsConfigurationPresenter.GetAcquirerDataShareRequestReturnedWithCommentsTemplateId()
    {
        var acquirerDataShareRequestReturnedWithCommentsTemplateId = DoGetAcquirerDataShareRequestReturnedWithCommentsTemplateId();

        return Guid.Parse(acquirerDataShareRequestReturnedWithCommentsTemplateId);
    }

    string INotificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminEmailAddress()
    {
        return DoGetDataShareRequestNotificationCddoAdminEmailAddress();
    }

    string INotificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminName()
    {
        return DoGetDataShareRequestNotificationCddoAdminName();
    }

    IEnumerable<SettingValue> INotificationsConfigurationPresenter.GetAllSettingValues()
    {
        return new List<SettingValue>
        {
            DoGetSettingValue("Gov Notify Api Key", DoGetGovNotifyApiKey),

            DoGetSettingValue("New Data Share Request Received Template Id", DoGetSupplierNewDataShareRequestReceivedTemplateId),
            DoGetSettingValue("Data Share Request Cancelled Template Id", DoGetSupplierDataShareRequestCancelledTemplateId),
            DoGetSettingValue("Data Share Request Accepted Template Id", DoGetAcquirerDataShareRequestAcceptedTemplateId),
            DoGetSettingValue("Data Share Request Rejected Template Id", DoGetAcquirerDataShareRequestRejectedTemplateId),
            DoGetSettingValue("Data Share Request Returned With Comments Template Id", DoGetAcquirerDataShareRequestReturnedWithCommentsTemplateId),

            DoGetSettingValue("Data Share Request Notification Cddo Admin Email Address", DoGetDataShareRequestNotificationCddoAdminEmailAddress),
            DoGetSettingValue("Data Share Request Notification Cddo Admin Name", DoGetDataShareRequestNotificationCddoAdminName)
        };

        static SettingValue DoGetSettingValue(string description, Func<string> getSettingValueFunc)
        {

            return new SettingValue
            {
                Description = description,
                Value = GetSettingValue()
            };

            string GetSettingValue()
            {
                try
                {
                    return getSettingValueFunc();
                }
                catch (Exception ex)
                {
                    return $"ERROR: {ex.Message}";
                }
            }
        }
    }

    private string DoGetGovNotifyApiKey()
    {
        return serviceConfigurationPresenter.GetValueInSection(notificationsSectionName, "gov_notify_api_key");
    }

    private string DoGetSupplierNewDataShareRequestReceivedTemplateId()
    {
        return serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [notificationsSectionName, templatesSectionName],
            "new_data_share_request_received_template_id");
    }

    private string DoGetSupplierDataShareRequestCancelledTemplateId()
    {
        return serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [notificationsSectionName, templatesSectionName],
            "data_share_request_cancelled_template_id");
    }

    private string DoGetAcquirerDataShareRequestAcceptedTemplateId()
    {
        return serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [notificationsSectionName, templatesSectionName],"data_share_request_accepted_template_id");
    }

    private string DoGetAcquirerDataShareRequestRejectedTemplateId()
    {
        return serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [notificationsSectionName, templatesSectionName],
            "data_share_request_rejected_template_id");
    }

    private string DoGetAcquirerDataShareRequestReturnedWithCommentsTemplateId()
    {
        return serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [notificationsSectionName, templatesSectionName],
            "data_share_request_returned_with_comments_template_id");
    }

    private string DoGetDataShareRequestNotificationCddoAdminEmailAddress()
    {
        return serviceConfigurationPresenter.GetValueInSection(
            notificationsSectionName, "cddo_admin_supplier_dsr_notification_email_address");
    }

    private string DoGetDataShareRequestNotificationCddoAdminName()
    {
        return serviceConfigurationPresenter.GetValueInSection(
            notificationsSectionName, "cddo_admin_supplier_dsr_notification_name");
    }
}