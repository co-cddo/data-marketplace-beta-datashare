using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Db.Configuration;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;

namespace Agrimetrics.DataShare.Api.Logic.Services.Admin;

internal class AdminService(
    IDatabaseConnectionsConfigurationPresenter databaseConnectionsConfigurationPresenter,
    INotificationsConfigurationPresenter notificationsConfigurationPresenter,
    IUsersServiceConfigurationPresenter usersServiceConfigurationPresenter,
    IDataAssetInformationServiceConfigurationPresenter dataAssetInformationServiceConfigurationPresenter,
    IPageLinksConfigurationPresenter pageLinksConfigurationPresenter,
    IServiceOperationResultFactory serviceOperationResultFactory) : IAdminService
{
    async Task<IServiceOperationDataResult<SettingValueSet>> IAdminService.GetAllSettingsAsync()
    {
        var databaseConnectionSettingValues = databaseConnectionsConfigurationPresenter.GetAllSettingValues();
        var notificationsSettingValues = notificationsConfigurationPresenter.GetAllSettingValues();
        var userServiceSettingValues = usersServiceConfigurationPresenter.GetAllSettings();
        var datasetInformationSettingValues = dataAssetInformationServiceConfigurationPresenter.GetAllSettings();
        var pageLinksSettingValues = pageLinksConfigurationPresenter.GetAllSettings();

        var settingValueSet =  new SettingValueSet
        {
            DatabaseConnectionSettingValues = databaseConnectionSettingValues.ToList(),
            NotificationsSettingValues = notificationsSettingValues.ToList(),
            UserServiceSettingValues = userServiceSettingValues.ToList(),
            DatasetInformationSettingValues = datasetInformationSettingValues.ToList(),
            PageLinksSettingValues = pageLinksSettingValues.ToList()
        };

        return await Task.Run(() =>
            serviceOperationResultFactory.CreateSuccessfulDataResult(settingValueSet));
    }
}