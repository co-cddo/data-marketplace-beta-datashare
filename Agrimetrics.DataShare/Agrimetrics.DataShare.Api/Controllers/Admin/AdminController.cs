using Agrimetrics.DataShare.Api.Dto.Requests.Admin;
using Agrimetrics.DataShare.Api.Dto.Responses.Admin;
using Agrimetrics.DataShare.Api.Logic.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrimetrics.DataShare.Api.Controllers.Admin
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AdminController(
        ILogger<AdminController> logger,
        IAdminService adminService,
        IAdminResponseFactory adminResponseFactory) : ControllerBase
    {
        [HttpGet("GetAllSettings")]
        [ProducesResponseType(typeof(GetAllSettingsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSettings([FromQuery] GetAllSettingsRequest getAllSettingsRequest)
        {
            ArgumentNullException.ThrowIfNull(getAllSettingsRequest);

            try
            {
                var getSettingsReportResult = await adminService.GetAllSettingsAsync();

                if (!getSettingsReportResult.Success)
                {
                    var error = getSettingsReportResult.Error;

                    logger.LogError("Failed to GetAllSettings from AdminService: {Error}", error);

                    return BadRequest(error);
                }

                var response = adminResponseFactory.CreateGetAllSettingsResponse(
                    getAllSettingsRequest,
                    getSettingsReportResult.Data!);

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
