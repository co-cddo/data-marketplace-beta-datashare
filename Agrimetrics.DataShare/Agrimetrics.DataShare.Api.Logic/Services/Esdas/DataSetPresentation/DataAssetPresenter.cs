using System.Text.Json;
using System.Text.Json.Serialization;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;

public class DataAssetPresenter(
    ILogger<DataAssetPresenter> logger,
    IUserIdPresenter userIdPresenter,
    IDataAssetInformationServiceConfigurationPresenter dataAssetInformationServiceConfigurationPresenter) : IDataAssetPresenter
{
    async Task<GetEsdaOwnershipDetailsResponse> IDataAssetPresenter.GetEsdaOwnershipDetailsAsync(Guid dataSetId)
    {
        try
        {
            var getEsdaOwnershipDetailsEndPoint = dataAssetInformationServiceConfigurationPresenter.GetEsdaOwnershipDetailsEndPoint();
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            return await getEsdaOwnershipDetailsEndPoint
                .WithSettings(x =>
                    x.JsonSerializer = new DefaultJsonSerializer(new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() },
                        PropertyNameCaseInsensitive = true
                    }))
                .WithOAuthBearerToken(initiatingUserIdToken)
                .AppendQueryParam("DataAssetId", dataSetId)
                .GetJsonAsync<GetEsdaOwnershipDetailsResponse>();
        }
        catch (FlurlHttpException ex)
        {
            throw await HandleFlurlExceptionAsync(ex, $"Error in GetEsdaOwnershipDetailsAsync for dataSetId '{dataSetId}'");
        }
    }

    private async Task<DataSetFetchException> HandleFlurlExceptionAsync(
        FlurlHttpException ex,
        string messageBody)
    {
        var dataShareRequestException = await BuildDataSetFetchException();

        logger.LogError(ex, "{MessageBody}: {DataShareRequestException}", messageBody, dataShareRequestException);

        return dataShareRequestException;

        async Task<DataSetFetchException> BuildDataSetFetchException()
        {
            return new DataSetFetchException
            {
                StatusCode = ex.StatusCode,
                ResponseText = await ex.GetResponseStringAsync(),
                ExceptionText = ex.Message
            };
        }
    }
}