using Agrimetrics.DataShare.Api.Dto.Requests.Reporting;
using Agrimetrics.DataShare.Api.Dto.Responses.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.Reporting;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ReportingController(
    ILogger<ReportingController> logger,
    IReportingService reportingService,
    IReportingResponseFactory reportingResponseFactory) : ControllerBase
{
    [HttpPost("QueryDataShareRequestCounts")]
    [ProducesResponseType(typeof(QueryDataShareRequestsCountsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryDataShareRequestCounts(QueryDataShareRequestCountsRequest queryDataShareRequestCountsRequest)
    {
        ArgumentNullException.ThrowIfNull(queryDataShareRequestCountsRequest);

        try
        {
            var queryDataShareRequestCountsResult = await reportingService.QueryDataShareRequestCountsAsync(
                queryDataShareRequestCountsRequest.DataShareRequestCountQueries);

            if (!queryDataShareRequestCountsResult.Success)
            {
                var error = queryDataShareRequestCountsResult.Error;

                logger.LogError("Failed to QueryDataShareRequestCounts with ReportingService: {Error}", error);

                return BadRequest(error);
            }

            var response = reportingResponseFactory.CreateQueryDataShareRequestsCountsResponse(
                queryDataShareRequestCountsResult.Data!.DataShareRequestCounts);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: ex.Message);
            return BadRequest(ex.Message);
        }
    }
}