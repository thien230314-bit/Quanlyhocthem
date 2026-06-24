using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetTeacherTeachingSchedule;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller lịch dạy cho giảng viên
    [ApiController]
    [Route("api/teacher/schedules")]
    [Authorize(Roles = "Teacher")]
    public class TeacherSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/teacher/schedules/weekly
        // Giảng viên xem lịch dạy tuần của mình
        [HttpGet("weekly")]
        public async Task<IActionResult> GetMyTeachingSchedule(
            [FromQuery] DateTime? weekDate,
            [FromQuery] Guid? courseId)
        {
            var result = await _mediator.Send(new GetTeacherTeachingScheduleQuery
            {
                TeacherUserId = GetCurrentUserId(),
                WeekDate = weekDate,
                CourseId = courseId
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
                message = "Lấy lịch dạy tuần thành công.",
                data = result.Data
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