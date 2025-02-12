using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation
{
    public interface IDataAssetPresenter
    {
        Task<GetEsdaOwnershipDetailsResponse> GetEsdaOwnershipDetailsAsync(Guid dataSetId);
    }
}
