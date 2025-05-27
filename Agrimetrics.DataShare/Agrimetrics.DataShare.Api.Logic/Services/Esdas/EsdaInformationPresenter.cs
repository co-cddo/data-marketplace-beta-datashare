using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
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

            // TODO: IMPLEMENT: Stubbed the call to the data asset presenter
            //   var getEsdaOwnershipDetailsResponse = await dataAssetPresenter.GetEsdaOwnershipDetailsAsync(esdaId);
            var title = "";
            if (esdaId == Guid.Parse("8d085327-21b6-4d8b-9705-88faad231d23") ) {
                title = "Advance Passenger Information";
            }
            else
            {
                title = "Advance Passenger Information II";
            }
            var getEsdaOwnershipDetailsResponse = new GetEsdaOwnershipDetailsResponse
            {
                EsdaId = esdaId,
                Title = title,
                OrganisationId = 1,
                DomainId = 1,
                ContactPointName = "Rob Nichols",
                ContactPointEmailAddress = "robert.nichols@digital.cabinet-office.gov.uk",
                DataShareRequestNotificationRecipientType = DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress,
                CustomDsrNotificationAddress = "custom@example.com"
            };
            // END stub

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