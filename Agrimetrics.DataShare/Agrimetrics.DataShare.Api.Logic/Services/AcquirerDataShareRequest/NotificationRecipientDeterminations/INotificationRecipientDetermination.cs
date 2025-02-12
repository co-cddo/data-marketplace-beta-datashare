using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations
{
    public interface IDataShareRequestNotificationRecipientDetermination
    {
        Task<IDataShareRequestNotificationRecipient> DetermineDataShareRequestNotificationRecipientAsync(
            DataShareRequestNotificationInformationModelData dataShareRequestNotificationInformation);
    }
}
