using System.Diagnostics;
using Agrimetrics.DataShare.Api.Dto.Requests.Supplier;
using Agrimetrics.DataShare.Api.Dto.Responses.Supplier;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;

[ApiController]
[Authorize]
[Route("[controller]")]
public class SupplierDataShareRequestController(
    ILogger<SupplierDataShareRequestController> logger,
    ISupplierDataShareRequestService supplierDataShareRequestService,
    ISupplierDataShareRequestResponseFactory supplierDataShareRequestResponseFactory)
    : ControllerBase
{
    [HttpGet("GetSubmissionSummaries")]
    [ProducesResponseType(typeof(GetSubmissionSummariesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionSummaries([FromQuery] GetSubmissionSummariesRequest getSubmissionSummariesRequest)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionSummariesRequest);

        try
        {
            var getSubmissionSummariesResult = await supplierDataShareRequestService.GetSubmissionSummariesAsync();

            if (!getSubmissionSummariesResult.Success)
            {
                var error = getSubmissionSummariesResult.Error;

                logger.LogError("Failed to GetSubmissionSummaries from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetSubmissionSummariesResponse(
                getSubmissionSummariesResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetSubmissionInformation")]
    [ProducesResponseType(typeof(GetSubmissionInformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionInformation([FromQuery] GetSubmissionInformationRequest getSubmissionInformationRequest)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionInformationRequest);

        try
        {
            var getSubmissionInformationResult = await supplierDataShareRequestService.GetSubmissionInformationAsync(
                getSubmissionInformationRequest.DataShareRequestId);

            if (!getSubmissionInformationResult.Success)
            {
                var error = getSubmissionInformationResult.Error;

                logger.LogError("Failed to GetSubmissionInformation from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
                getSubmissionInformationRequest,
                getSubmissionInformationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("StartSubmissionReview")]
    [ProducesResponseType(typeof(StartSubmissionReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartSubmissionReview(StartSubmissionReviewRequest startSubmissionReviewRequest)
    {
        ArgumentNullException.ThrowIfNull(startSubmissionReviewRequest);

        try
        {
            var startSubmissionReviewResult = await supplierDataShareRequestService.StartSubmissionReviewAsync(
                startSubmissionReviewRequest.DataShareRequestId);

            if (!startSubmissionReviewResult.Success)
            {
                var error = startSubmissionReviewResult.Error;

                logger.LogError("Failed to StartSubmissionReview with SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
                startSubmissionReviewRequest,
                startSubmissionReviewResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetSubmissionReviewInformation")]
    [ProducesResponseType(typeof(GetSubmissionReviewInformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionReviewInformation([FromQuery] GetSubmissionReviewInformationRequest getSubmissionReviewInformationRequest)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionReviewInformationRequest);

        try
        {
            var getSubmissionReviewInformationResult = await supplierDataShareRequestService.GetSubmissionReviewInformationAsync(
                getSubmissionReviewInformationRequest.DataShareRequestId);

            if (!getSubmissionReviewInformationResult.Success)
            {
                var error = getSubmissionReviewInformationResult.Error;

                logger.LogError("Failed to GetSubmissionReviewInformation from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
                getSubmissionReviewInformationRequest,
                getSubmissionReviewInformationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetSubmissionDetails")]
    [ProducesResponseType(typeof(GetSubmissionDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionDetails([FromQuery] GetSubmissionDetailsRequest getSubmissionDetailsRequest)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionDetailsRequest);

        try
        {
            var getSubmissionDetailsResult = await supplierDataShareRequestService.GetSubmissionDetailsAsync(
                getSubmissionDetailsRequest.DataShareRequestId);

            if (!getSubmissionDetailsResult.Success)
            {
                var error = getSubmissionDetailsResult.Error;

                logger.LogError("Failed to GetSubmissionDetails from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
                getSubmissionDetailsRequest,
                getSubmissionDetailsResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetSubmissionContentAsFile")]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionContentAsFile([FromQuery] GetSubmissionAsFileRequest getSubmissionAsFileRequest)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionAsFileRequest);

        try
        {
            var getSubmissionContentAsFileResult = await supplierDataShareRequestService.GetSubmissionContentAsFileAsync(
                getSubmissionAsFileRequest.DataShareRequestId,
                getSubmissionAsFileRequest.FileFormat);

            if (!getSubmissionContentAsFileResult.Success)
            {
                var error = getSubmissionContentAsFileResult.Error;

                logger.LogError("Failed to GetSubmissionContentAsFile from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var submissionContentAsFile = getSubmissionContentAsFileResult.Data!;
            return File(submissionContentAsFile.Content, submissionContentAsFile.ContentType, submissionContentAsFile.FileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetReturnedSubmissionInformation")]
    [ProducesResponseType(typeof(GetReturnedSubmissionInformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetReturnedSubmissionInformation([FromQuery] GetReturnedSubmissionInformationRequest getReturnedSubmissionInformationRequest)
    {
        ArgumentNullException.ThrowIfNull(getReturnedSubmissionInformationRequest);

        try
        {
            var getReturnedSubmissionInformationResult = await supplierDataShareRequestService.GetReturnedSubmissionInformationAsync(
                getReturnedSubmissionInformationRequest.DataShareRequestId);

            if (!getReturnedSubmissionInformationResult.Success)
            {
                var error = getReturnedSubmissionInformationResult.Error;

                logger.LogError("Failed to GetReturnedSubmissionInformation from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
                getReturnedSubmissionInformationRequest,
                getReturnedSubmissionInformationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetCompletedSubmissionInformation")]
    [ProducesResponseType(typeof(GetCompletedSubmissionInformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompletedSubmissionInformation([FromQuery] GetCompletedSubmissionInformationRequest getCompletedSubmissionInformationRequest)
    {
        ArgumentNullException.ThrowIfNull(getCompletedSubmissionInformationRequest);

        try
        {
            var getCompletedSubmissionInformationResult = await supplierDataShareRequestService.GetCompletedSubmissionInformationAsync(
                getCompletedSubmissionInformationRequest.DataShareRequestId);

            if (!getCompletedSubmissionInformationResult.Success)
            {
                var error = getCompletedSubmissionInformationResult.Error;

                logger.LogError("Failed to GetCompletedSubmissionInformation from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
                getCompletedSubmissionInformationRequest,
                getCompletedSubmissionInformationResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("SetSubmissionNotes")]
    [ProducesResponseType(typeof(SetSubmissionNotesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetSubmissionNotes(SetSubmissionNotesRequest setSubmissionNotesRequest)
    {
        ArgumentNullException.ThrowIfNull(setSubmissionNotesRequest);

        try
        {
            var setSubmissionNotesResult = await supplierDataShareRequestService.SetSubmissionNotesAsync(
                setSubmissionNotesRequest.DataShareRequestId,
                setSubmissionNotesRequest.Notes);

            if (!setSubmissionNotesResult.Success)
            {
                var error = setSubmissionNotesResult.Error;

                logger.LogError("Failed to SetSubmissionNotes with SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateSetSubmissionNotesResponse(
                setSubmissionNotesRequest);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("AcceptSubmission")]
    [ProducesResponseType(typeof(AcceptSubmissionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptSubmission(AcceptSubmissionRequest acceptSubmissionRequest)
    {
        ArgumentNullException.ThrowIfNull(acceptSubmissionRequest);

        try
        {
            var acceptSubmissionResult = await supplierDataShareRequestService.AcceptSubmissionAsync(
                acceptSubmissionRequest.DataShareRequestId,
                acceptSubmissionRequest.CommentsToAcquirer);

            if (!acceptSubmissionResult.Success)
            {
                var error = acceptSubmissionResult.Error;

                logger.LogError("Failed to AcceptSubmission from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
                acceptSubmissionRequest,
                acceptSubmissionResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("RejectSubmission")]
    [ProducesResponseType(typeof(RejectSubmissionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectSubmission(RejectSubmissionRequest rejectSubmissionRequest)
    {
        ArgumentNullException.ThrowIfNull(rejectSubmissionRequest);

        try
        {
            var rejectSubmissionResult = await supplierDataShareRequestService.RejectSubmissionAsync(
                rejectSubmissionRequest.DataShareRequestId,
                rejectSubmissionRequest.CommentsToAcquirer);

            if (!rejectSubmissionResult.Success)
            {
                var error = rejectSubmissionResult.Error;

                logger.LogError("Failed to RejectSubmission from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
                rejectSubmissionRequest,
                rejectSubmissionResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ReturnSubmission")]
    [ProducesResponseType(typeof(ReturnSubmissionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReturnSubmission(ReturnSubmissionRequest returnSubmissionRequest)
    {
        ArgumentNullException.ThrowIfNull(returnSubmissionRequest);

        try
        {
            var returnSubmissionResult = await supplierDataShareRequestService.ReturnSubmissionAsync(
                returnSubmissionRequest.DataShareRequestId,
                returnSubmissionRequest.CommentsToAcquirer);

            if (!returnSubmissionResult.Success)
            {
                var error = returnSubmissionResult.Error;

                logger.LogError("Failed to ReturnSubmission from SupplierDataShareRequestService: {Error}", error);

                return BadRequest(error);
            }

            var response = supplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
                returnSubmissionRequest,
                returnSubmissionResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }
}