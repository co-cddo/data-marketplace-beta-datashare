using Agrimetrics.DataShare.Api.Dto.Requests.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Responses.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.AuditLogs
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuditLogController(
        ILogger<AuditLogController> logger,
        IAuditLogService auditLogService,
        IAuditLogResponseFactory auditLogResponseFactory) : ControllerBase
    {

        [HttpGet("GetDataShareRequestAuditLog")]
        [ProducesResponseType(typeof(GetDataShareRequestAuditLogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDataShareRequestAuditLog([FromQuery] GetDataShareRequestAuditLogRequest getDataShareRequestAuditLogRequest)
        {
            ArgumentNullException.ThrowIfNull(getDataShareRequestAuditLogRequest);

            try
            {
                var getAuditLogsForDataShareRequestStatusChangeToStatusResult = await auditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                    getDataShareRequestAuditLogRequest.DataShareRequestId,
                    getDataShareRequestAuditLogRequest.ToStatuses);

                if (!getAuditLogsForDataShareRequestStatusChangeToStatusResult.Success)
                {
                    var error = getAuditLogsForDataShareRequestStatusChangeToStatusResult.Error;

                    logger.LogError("Failed to GetAuditLogsForDataShareRequestStatusChangeToStatus from AuditLogService: {Error}", error);

                    return BadRequest(error);
                }

                var response = auditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
                    getDataShareRequestAuditLogRequest,
                    getAuditLogsForDataShareRequestStatusChangeToStatusResult.Data!);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
