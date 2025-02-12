using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Dto.Requests.Admin;
using Agrimetrics.DataShare.Api.Dto.Responses.Admin;

namespace Agrimetrics.DataShare.Api.Controllers.Admin;

public interface IAdminResponseFactory
{
    GetAllSettingsResponse CreateGetAllSettingsResponse(
        GetAllSettingsRequest getAllSettingsRequest,
        SettingValueSet settingValueSet);
}