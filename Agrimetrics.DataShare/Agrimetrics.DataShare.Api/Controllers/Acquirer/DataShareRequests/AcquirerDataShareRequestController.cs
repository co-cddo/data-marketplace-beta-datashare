using Agrimetrics.DataShare.Api.Dto.Requests.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;

[ApiController]
[Authorize]
[Route("[controller]")]
public class AcquirerDataShareRequestController(
    ILogger<AcquirerDataShareRequestController> logger,
    IAcquirerDataShareRequestService acquirerDataShareRequestService,
    IAcquirerDataShareRequestResponseFactory acquirerResponseFactory)
    : ControllerBase
{
    [HttpGet("GetEsdaQuestionSetOutline")]
    [ProducesResponseType(typeof(GetEsdaQuestionSetOutlineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEsdaQuestionSetOutline([FromQuery] GetEsdaQuestionSetOutlineRequest getEsdaQuestionSetOutlineRequest)
    {
        ArgumentNullException.ThrowIfNull(getEsdaQuestionSetOutlineRequest);

        try
        {
            var getEsdaQuestionSetOutlineResult = await acquirerDataShareRequestService.GetEsdaQuestionSetOutlineRequestAsync(
                getEsdaQuestionSetOutlineRequest.SupplierDomainId,
                getEsdaQuestionSetOutlineRequest.SupplierOrganisationId,
                getEsdaQuestionSetOutlineRequest.EsdaId);

            if (!getEsdaQuestionSetOutlineResult.Success)
            {
                logger.LogError("Failed to GetEsdaQuestionSetOutline from AcquirerDataShareRequestService: {Error}",
                    getEsdaQuestionSetOutlineResult.Error);
                return BadRequest(getEsdaQuestionSetOutlineResult.Error);
            }

            var response = acquirerResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
                getEsdaQuestionSetOutlineRequest,
                getEsdaQuestionSetOutlineResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("StartDataShareRequest")]
    [ProducesResponseType(typeof(StartDataShareRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartDataShareRequest(StartDataShareRequestRequest startDataShareRequestRequest)
    {
        ArgumentNullException.ThrowIfNull(startDataShareRequestRequest);

        try
        {
            var startDataShareRequestResult = await acquirerDataShareRequestService.StartDataShareRequestAsync(
                startDataShareRequestRequest.EsdaId);

            if (!startDataShareRequestResult.Success)
            {
                var error = startDataShareRequestResult.Error;

                logger.LogError("Failed to StartDataShareRequest with AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateStartDataShareRequestResponse(
                startDataShareRequestRequest, startDataShareRequestResult.Data);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestSummaries")]
    [ProducesResponseType(typeof(GetDataShareRequestSummariesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDataShareRequestSummaries([FromQuery] GetDataShareRequestSummariesRequest getDataShareRequestSummariesRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestSummariesRequest);

        try
        {
            var getDataShareRequestSummariesResult = await acquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                getDataShareRequestSummariesRequest.AcquirerUserId,
                getDataShareRequestSummariesRequest.AcquirerDomainId,
                getDataShareRequestSummariesRequest.AcquirerOrganisationId,
                getDataShareRequestSummariesRequest.SupplierDomainId,
                getDataShareRequestSummariesRequest.SupplierOrganisationId,
                getDataShareRequestSummariesRequest.EsdaId,
                getDataShareRequestSummariesRequest.DataShareRequestStatuses);

            if (!getDataShareRequestSummariesResult.Success)
            {
                var error = getDataShareRequestSummariesResult.Error;

                logger.LogError("Failed to GetDataShareRequestSummaries from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestSummariesResponse(
                getDataShareRequestSummariesResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestAdminSummaries")]
    [ProducesResponseType(typeof(GetDataShareRequestAdminSummariesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDataShareRequestAdminSummaries([FromQuery] GetDataShareRequestAdminSummariesRequest getDataShareRequestAdminSummariesRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestAdminSummariesRequest);

        try
        {
            var getDataShareRequestAdminSummariesResult = await acquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
                getDataShareRequestAdminSummariesRequest.AcquirerOrganisationId,
                getDataShareRequestAdminSummariesRequest.SupplierOrganisationId,
                getDataShareRequestAdminSummariesRequest.DataShareRequestStatuses);

            if (!getDataShareRequestAdminSummariesResult.Success)
            {
                var error = getDataShareRequestAdminSummariesResult.Error;

                logger.LogError("Failed to GetDataShareRequestAdminSummaries from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestAdminSummariesResponse(
                getDataShareRequestAdminSummariesResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetAcquirerDataShareRequestSummaries")]
    [ProducesResponseType(typeof(GetDataShareRequestSummariesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAcquirerDataShareRequestSummaries([FromQuery] GetAcquirerDataShareRequestSummariesRequest getAcquirerDataShareRequestSummariesRequest)
    {
        ArgumentNullException.ThrowIfNull(getAcquirerDataShareRequestSummariesRequest);

        try
        {
            var getDataShareRequestSummariesResult = await acquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                getAcquirerDataShareRequestSummariesRequest.SupplierDomainId,
                getAcquirerDataShareRequestSummariesRequest.SupplierOrganisationId,
                getAcquirerDataShareRequestSummariesRequest.EsdaId,
                getAcquirerDataShareRequestSummariesRequest.DataShareRequestStatuses);

            if (!getDataShareRequestSummariesResult.Success)
            {
                var error = getDataShareRequestSummariesResult.Error;

                logger.LogError("Failed to GetAcquirerDataShareRequestSummaries from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestSummariesResponse(
                getDataShareRequestSummariesResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation")]
    [ProducesResponseType(typeof(GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation([FromQuery] GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        try
        {
            var getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResult = await acquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest.EsdaId);

            if (!getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResult.Success)
            {
                var error = getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResult.Error;

                logger.LogError("Failed to GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest,
                getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestQuestionsSummary")]
    [ProducesResponseType(typeof(GetDataShareRequestQuestionsSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataShareRequestQuestionsSummary([FromQuery] GetDataShareRequestQuestionsSummaryRequest getDataShareRequestQuestionsSummaryRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestQuestionsSummaryRequest);

        try
        {
            var getDataShareRequestQuestionsSummaryResult = await acquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                    getDataShareRequestQuestionsSummaryRequest.DataShareRequestId);

            if (!getDataShareRequestQuestionsSummaryResult.Success)
            {
                var error = getDataShareRequestQuestionsSummaryResult.Error;

                logger.LogError("Failed to GetDataShareRequestQuestionsSummary from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
                getDataShareRequestQuestionsSummaryRequest, getDataShareRequestQuestionsSummaryResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestQuestionInformation")]
    [ProducesResponseType(typeof(GetDataShareRequestQuestionInformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataShareRequestQuestionInformation([FromQuery] GetDataShareRequestQuestionInformationRequest getDataShareRequestQuestionInformationRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestQuestionInformationRequest);

        try
        {
            var getDataShareRequestQuestionInformationResult = await acquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
                getDataShareRequestQuestionInformationRequest.DataShareRequestId,
                getDataShareRequestQuestionInformationRequest.QuestionId);

            if (!getDataShareRequestQuestionInformationResult.Success)
            {
                var error = getDataShareRequestQuestionInformationResult.Error;

                logger.LogError("Failed to GetDataShareRequestQuestionInformation from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
                getDataShareRequestQuestionInformationRequest, getDataShareRequestQuestionInformationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("SetDataShareRequestQuestionAnswer")]
    [ProducesResponseType(typeof(SetDataShareRequestQuestionAnswerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetDataShareRequestQuestionAnswer(SetDataShareRequestQuestionAnswerRequest setDataShareRequestQuestionAnswerRequest)
    {
        ArgumentNullException.ThrowIfNull(setDataShareRequestQuestionAnswerRequest);

        try
        {
            var setDataShareRequestQuestionAnswerResult = await acquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                setDataShareRequestQuestionAnswerRequest.DataShareRequestQuestionAnswer);

            if (!setDataShareRequestQuestionAnswerResult.Success)
            {
                var error = setDataShareRequestQuestionAnswerResult.Error;

                logger.LogError("Failed to SetDataShareRequestQuestionAnswer with AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateSetDataShareRequestQuestionAnswerResponse(
                setDataShareRequestQuestionAnswerRequest, setDataShareRequestQuestionAnswerResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetDataShareRequestAnswersSummary")]
    [ProducesResponseType(typeof(GetDataShareRequestAnswersSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataShareRequestAnswersSummary([FromQuery] GetDataShareRequestAnswersSummaryRequest getDataShareRequestAnswersSummaryRequest)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestAnswersSummaryRequest);

        try
        {
            var getDataShareRequestAnswersSummaryResult = await acquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(
                getDataShareRequestAnswersSummaryRequest.DataShareRequestId);

            if (!getDataShareRequestAnswersSummaryResult.Success)
            {
                var error = getDataShareRequestAnswersSummaryResult.Error;

                logger.LogError("Failed to GetDataShareRequestAnswersSummary from AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
                getDataShareRequestAnswersSummaryRequest, getDataShareRequestAnswersSummaryResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("SubmitDataShareRequest")]
    [ProducesResponseType(typeof(SubmitDataShareRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitDataShareRequest(SubmitDataShareRequestRequest submitDataShareRequestRequest)
    {
        ArgumentNullException.ThrowIfNull(submitDataShareRequestRequest);

        try
        {
            var submitDataShareRequestResult = await acquirerDataShareRequestService.SubmitDataShareRequestAsync(
                submitDataShareRequestRequest.DataShareRequestId);

            if (!submitDataShareRequestResult.Success)
            {
                var error = submitDataShareRequestResult.Error;

                logger.LogError("Failed to SubmitDataShareRequest with AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateSubmitDataShareRequestResponse(
                submitDataShareRequestRequest, submitDataShareRequestResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("CancelDataShareRequest")]
    [ProducesResponseType(typeof(CancelDataShareRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelDataShareRequest(CancelDataShareRequestRequest cancelDataShareRequestRequest)
    {
        ArgumentNullException.ThrowIfNull(cancelDataShareRequestRequest);

        try
        {
            var cancelDataShareRequestResult = await acquirerDataShareRequestService.CancelDataShareRequestAsync(
                cancelDataShareRequestRequest.DataShareRequestId,
                cancelDataShareRequestRequest.ReasonsForCancellation);

            if (!cancelDataShareRequestResult.Success)
            {
                var error = cancelDataShareRequestResult.Error;

                logger.LogError("Failed to CancelDataShareRequest with AcquirerDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateCancelDataShareRequestResponse(
                cancelDataShareRequestRequest,
                cancelDataShareRequestResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteDataShareRequest")]
    [ProducesResponseType(typeof(DeleteDataShareRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDataShareRequest(DeleteDataShareRequestRequest deleteDataShareRequestRequest)
    {
        ArgumentNullException.ThrowIfNull(deleteDataShareRequestRequest);

        try
        {
            var deleteDataShareRequestResult = await acquirerDataShareRequestService.DeleteDataShareRequestAsync(
                deleteDataShareRequestRequest.DataShareRequestId);

            if (!deleteDataShareRequestResult.Success)
            {
                var error = deleteDataShareRequestResult.Error;

                logger.LogError("Failed to DeleteDataShareRequest with AcquirerDataShareRequestService: {Error}", error);

                return deleteDataShareRequestResult.StatusCode.HasValue
                    ? new ObjectResult(error) { StatusCode = (int)deleteDataShareRequestResult.StatusCode.Value }
                    : BadRequest(error);
            }

            var response = acquirerResponseFactory.CreateDeleteDataShareRequestResponse(
                deleteDataShareRequestRequest,
                deleteDataShareRequestResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }
}