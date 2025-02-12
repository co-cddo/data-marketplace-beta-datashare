using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas
{
    public interface IEsdaInformationPresenter
    {
        Task<IEsdaDetails> GetEsdaDetailsByIdAsync(Guid esdaId);
    }
}
