using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetStudentWeeklySchedule;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller lịch học cho học viên
    [ApiController]
    [Route("api/student/schedules")]
    [Authorize(Roles = "Student")]
    public class StudentSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/student/schedules/weekly
        // Học viên xem thời khóa biểu tuần của mình
        [HttpGet("weekly")]
        public async Task<IActionResult> GetMyWeeklySchedule([FromQuery] DateTime? weekDate)
        {
            var result = await _mediator.Send(new GetStudentWeeklyScheduleQuery
            {
                StudentUserId = GetCurrentUserId(),
                WeekDate = weekDate
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
                message = "Lấy thời khóa biểu tuần thành công.",
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