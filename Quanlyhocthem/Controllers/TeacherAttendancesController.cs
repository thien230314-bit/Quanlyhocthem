using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Attendances.Commands.OpenAttendanceSession;
using Quanlyhocthem.Application.Features.Attendances.Commands.UpdateAttendanceRecord;
using Quanlyhocthem.Application.Features.Attendances.Queries.GetAttendanceSessionRecords;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller điểm danh cho giảng viên
    [ApiController]
    [Route("api/teacher/attendances")]
    [Authorize(Roles = "Teacher")]
    public class TeacherAttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherAttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: /api/teacher/attendances/sessions
        // Giảng viên mở ca điểm danh
        [HttpPost("sessions")]
        public async Task<IActionResult> OpenAttendanceSession([FromBody] OpenAttendanceSessionCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();

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
                message = "Mở ca điểm danh thành công.",
                data = result.Data
            });
        }

        // GET: /api/teacher/attendances/sessions/{attendanceSessionId}/records
        // Giảng viên xem kết quả điểm danh
        [HttpGet("sessions/{attendanceSessionId}/records")]
        public async Task<IActionResult> GetAttendanceSessionRecords(Guid attendanceSessionId)
        {
            var result = await _mediator.Send(new GetAttendanceSessionRecordsQuery
            {
                TeacherUserId = GetCurrentUserId(),
                AttendanceSessionId = attendanceSessionId
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy kết quả điểm danh thành công.",
                data = result.Data
            });
        }

        // PUT: /api/teacher/attendances/{attendanceId}
        // Giảng viên sửa điểm danh thủ công
        [HttpPut("{attendanceId}")]
        public async Task<IActionResult> UpdateAttendanceRecord(Guid attendanceId, [FromBody] UpdateAttendanceRecordCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();
            command.AttendanceId = attendanceId;

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
                message = "Cập nhật điểm danh thành công."
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