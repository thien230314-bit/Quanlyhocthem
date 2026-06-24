using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Attendances.Commands.StudentCheckIn;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller điểm danh cho học viên
    [ApiController]
    [Route("api/student/attendances")]
    [Authorize(Roles = "Student")]
    public class StudentAttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentAttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: /api/student/attendances/check-in
        // Học viên điểm danh bằng mã do giảng viên cung cấp
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] StudentCheckInCommand command)
        {
            command.StudentUserId = GetCurrentUserId();

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Điểm danh thành công."
            });
        }

        // Lấy UserId từ JWT token
        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Không tìm thấy UserId trong token.");

            return Guid.Parse(userId);
        }
    }
}