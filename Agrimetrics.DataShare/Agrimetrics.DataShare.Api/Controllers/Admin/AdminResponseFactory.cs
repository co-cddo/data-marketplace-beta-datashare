using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Dto.Requests.Admin;
using Agrimetrics.DataShare.Api.Dto.Responses.Admin;

namespace Agrimetrics.DataShare.Api.Controllers.Admin;

internal class AdminResponseFactory : IAdminResponseFactory
{
    GetAllSettingsResponse IAdminResponseFactory.CreateGetAllSettingsResponse(
        GetAllSettingsRequest getAllSettingsRequest,
        SettingValueSet settingValueSet)
    {
        ArgumentNullException.ThrowIfNull(getAllSettingsRequest);
        ArgumentNullException.ThrowIfNull(settingValueSet);

        return new GetAllSettingsResponse
        {
            SettingValues = settingValueSet
        };
    }
}