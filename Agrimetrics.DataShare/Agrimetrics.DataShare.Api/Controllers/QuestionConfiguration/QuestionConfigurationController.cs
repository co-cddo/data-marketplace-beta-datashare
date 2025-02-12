using Agrimetrics.DataShare.Api.Dto.Requests.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Responses.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;

[ApiController]
[Route("[controller]")]
public class QuestionConfigurationController(
    ILogger<QuestionConfigurationController> logger,
    IQuestionConfigurationService questionConfigurationService,
    IQuestionConfigurationResponseFactory questionConfigurationResponseFactory)
    : ControllerBase
{
    #region Compulsory Questions
    [HttpGet("GetCompulsoryQuestions")]
    [ProducesResponseType(typeof(GetCompulsoryQuestionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompulsoryQuestions([FromQuery] GetCompulsoryQuestionsRequest getCompulsoryQuestionsRequest)
    {
        ArgumentNullException.ThrowIfNull(getCompulsoryQuestionsRequest);

        try
        {
            var getCompulsoryQuestionsResult = await questionConfigurationService.GetCompulsoryQuestionsAsync(
                getCompulsoryQuestionsRequest.RequestingUserId);

            if (!getCompulsoryQuestionsResult.Success)
            {
                var error = getCompulsoryQuestionsResult.Error;

                logger.LogError("Failed to GetCompulsoryQuestions from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
                getCompulsoryQuestionsRequest, getCompulsoryQuestionsResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("SetCompulsoryQuestion")]
    [ProducesResponseType(typeof(SetCompulsoryQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetCompulsoryQuestion(SetCompulsoryQuestionRequest setCompulsoryQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(setCompulsoryQuestionRequest);

        try
        {
            var setCompulsoryQuestionResult = await questionConfigurationService.SetCompulsoryQuestionAsync(
                setCompulsoryQuestionRequest.RequestingUserId,
                setCompulsoryQuestionRequest.QuestionId);

            if (!setCompulsoryQuestionResult.Success)
            {
                var error = setCompulsoryQuestionResult.Error;

                logger.LogError("Failed to SetCompulsoryQuestion from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateSetCompulsoryQuestionResponse(
                setCompulsoryQuestionRequest);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ClearCompulsoryQuestion")]
    [ProducesResponseType(typeof(ClearCompulsoryQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClearCompulsoryQuestion(ClearCompulsoryQuestionRequest clearCompulsoryQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(clearCompulsoryQuestionRequest);

        try
        {
            var clearCompulsoryQuestionResult = await questionConfigurationService.ClearCompulsoryQuestionAsync(
                clearCompulsoryQuestionRequest.RequestingUserId,
                clearCompulsoryQuestionRequest.QuestionId);

            if (!clearCompulsoryQuestionResult.Success)
            {
                var error = clearCompulsoryQuestionResult.Error;

                logger.LogError("Failed to ClearCompulsoryQuestion from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateClearCompulsoryQuestionResponse(
                clearCompulsoryQuestionRequest);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Compulsory Supplier-Mandated Questions
    [HttpGet("GetCompulsorySupplierMandatedQuestions")]
    [ProducesResponseType(typeof(GetCompulsorySupplierMandatedQuestionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompulsorySupplierMandatedQuestions([FromQuery] GetCompulsorySupplierMandatedQuestionsRequest getCompulsorySupplierMandatedQuestionsRequest)
    {
        ArgumentNullException.ThrowIfNull(getCompulsorySupplierMandatedQuestionsRequest);

        try
        {
            var getCompulsorySupplierMandatedQuestionsResult = await questionConfigurationService.GetCompulsorySupplierMandatedQuestionsAsync(
                getCompulsorySupplierMandatedQuestionsRequest.RequestingUserId,
                getCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId);

            if (!getCompulsorySupplierMandatedQuestionsResult.Success)
            {
                var error = getCompulsorySupplierMandatedQuestionsResult.Error;

                logger.LogError("Failed to GetCompulsorySupplierMandatedQuestions from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
                getCompulsorySupplierMandatedQuestionsRequest, getCompulsorySupplierMandatedQuestionsResult.Data!);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("SetCompulsorySupplierMandatedQuestion")]
    [ProducesResponseType(typeof(SetCompulsorySupplierMandatedQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetCompulsorySupplierMandatedQuestion(SetCompulsorySupplierMandatedQuestionRequest setCompulsorySupplierMandatedQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(setCompulsorySupplierMandatedQuestionRequest);

        try
        {
            var setCompulsorySupplierMandatedQuestionResult = await questionConfigurationService.SetCompulsorySupplierMandatedQuestionAsync(
                setCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                setCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                setCompulsorySupplierMandatedQuestionRequest.QuestionId);

            if (!setCompulsorySupplierMandatedQuestionResult.Success)
            {
                var error = setCompulsorySupplierMandatedQuestionResult.Error;

                logger.LogError("Failed to SetCompulsorySupplierMandatedQuestion from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateSetCompulsorySupplierMandatedQuestionResponse(
                setCompulsorySupplierMandatedQuestionRequest);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ClearCompulsorySupplierMandatedQuestion")]
    [ProducesResponseType(typeof(ClearCompulsorySupplierMandatedQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClearCompulsorySupplierMandatedQuestion(ClearCompulsorySupplierMandatedQuestionRequest clearCompulsorySupplierMandatedQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(clearCompulsorySupplierMandatedQuestionRequest);

        try
        {
            var clearCompulsorySupplierMandatedQuestionResult = await questionConfigurationService.ClearCompulsorySupplierMandatedQuestionAsync(
                clearCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                clearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                clearCompulsorySupplierMandatedQuestionRequest.QuestionId);

            if (!clearCompulsorySupplierMandatedQuestionResult.Success)
            {
                var error = clearCompulsorySupplierMandatedQuestionResult.Error;

                logger.LogError("Failed to ClearCompulsorySupplierMandatedQuestion from QuestionConfigurationService: {Error}", error);

                return BadRequest(error);
            }

            var response = questionConfigurationResponseFactory.CreateClearCompulsorySupplierMandatedQuestionResponse(
                clearCompulsorySupplierMandatedQuestionRequest);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }
    #endregion
}