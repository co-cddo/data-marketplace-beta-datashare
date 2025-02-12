using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas;

public class EsdaInformationPresenter(
    ILogger<EsdaInformationPresenter> logger,
    IDataAssetPresenter dataAssetPresenter) : IEsdaInformationPresenter
{
    async Task<IEsdaDetails> IEsdaInformationPresenter.GetEsdaDetailsByIdAsync(Guid esdaId)
    {
        logger.LogDebug("Getting details of ESDA with Id '{EsdaId}'", esdaId);

        try
        {
            var getEsdaOwnershipDetailsResponse = await dataAssetPresenter.GetEsdaOwnershipDetailsAsync(esdaId);

            return new EsdaDetails
            {
                Id = getEsdaOwnershipDetailsResponse.EsdaId,
                Title = getEsdaOwnershipDetailsResponse.Title,
                SupplierOrganisationId = getEsdaOwnershipDetailsResponse.OrganisationId,
                SupplierDomainId = getEsdaOwnershipDetailsResponse.DomainId,
                ContactPointName = getEsdaOwnershipDetailsResponse.ContactPointName,
                ContactPointEmailAddress = getEsdaOwnershipDetailsResponse.ContactPointEmailAddress,
                DataShareRequestNotificationRecipientType = getEsdaOwnershipDetailsResponse.DataShareRequestNotificationRecipientType,
                CustomDsrNotificationAddress = getEsdaOwnershipDetailsResponse.CustomDsrNotificationAddress
            };
        }
        catch (Exception ex)
        {
            var errorMessage = $"Failed to get details of ESDA with Id '{esdaId}'";

            logger.LogError(ex, errorMessage);

            throw new DataShareRequestGeneralException(errorMessage, ex);
        }
    }
}