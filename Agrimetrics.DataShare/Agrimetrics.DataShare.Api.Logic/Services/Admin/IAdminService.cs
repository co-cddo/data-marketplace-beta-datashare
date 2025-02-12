using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.Admin
{
    public interface IAdminService
    {
        Task<IServiceOperationDataResult<SettingValueSet>> GetAllSettingsAsync();
    }
}
