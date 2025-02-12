namespace Agrimetrics.DataShare.Api.Core.Configuration.Model;

public class SettingValueSet
{
    public List<SettingValue> DatabaseConnectionSettingValues { get; set; }
    public List<SettingValue> NotificationsSettingValues { get; set; }
    public List<SettingValue> UserServiceSettingValues { get; set; }
    public List<SettingValue> DatasetInformationSettingValues { get; set; }
    public List<SettingValue> PageLinksSettingValues { get; set; }
}