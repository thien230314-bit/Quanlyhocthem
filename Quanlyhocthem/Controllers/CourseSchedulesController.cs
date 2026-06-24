using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
using Quanlyhocthem.Application.Features.CourseSchedules.Commands.DeleteCourseSchedule;
using Quanlyhocthem.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
using Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseScheduleById;
using Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseSchedules;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller Admin quản lý lịch học / lịch dạy
    [ApiController]
    [Route("api/course-schedules")]
    [Authorize(Roles = "Admin")]
    public class CourseSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/course-schedules
        // Admin xem danh sách lịch học
        [HttpGet]
        public async Task<IActionResult> GetCourseSchedules(
            [FromQuery] Guid? courseId,
            [FromQuery] Guid? teacherId,
            [FromQuery] Guid? classroomId,
            [FromQuery] DayOfWeek? dayOfWeek,
            [FromQuery] bool? isActive)
        {
            var result = await _mediator.Send(new GetCourseSchedulesQuery
            {
                CourseId = courseId,
                TeacherId = teacherId,
                ClassroomId = classroomId,
                DayOfWeek = dayOfWeek,
                IsActive = isActive
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
                message = "Lấy danh sách lịch học thành công.",
                data = result.Data
            });
        }

        // GET: /api/course-schedules/{courseScheduleId}
        // Admin xem chi tiết lịch học
        [HttpGet("{courseScheduleId}")]
        public async Task<IActionResult> GetCourseScheduleById(Guid courseScheduleId)
        {
            var result = await _mediator.Send(new GetCourseScheduleByIdQuery
            {
                CourseScheduleId = courseScheduleId
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
                message = "Lấy chi tiết lịch học thành công.",
                data = result.Data
            });
        }

        // POST: /api/course-schedules
        // Admin tạo lịch học cho lớp
        [HttpPost]
        public async Task<IActionResult> CreateCourseSchedule([FromBody] CreateCourseScheduleCommand command)
        {
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
                message = "Tạo lịch học thành công.",
                courseScheduleId = result.Data
            });
        }

        // PUT: /api/course-schedules/{courseScheduleId}
        // Admin cập nhật lịch học
        [HttpPut("{courseScheduleId}")]
        public async Task<IActionResult> UpdateCourseSchedule(Guid courseScheduleId, [FromBody] UpdateCourseScheduleCommand command)
        {
            command.CourseScheduleId = courseScheduleId;

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
                message = "Cập nhật lịch học thành công."
            });
        }

        // DELETE: /api/course-schedules/{courseScheduleId}
        // Admin xóa lịch học
        [HttpDelete("{courseScheduleId}")]
        public async Task<IActionResult> DeleteCourseSchedule(Guid courseScheduleId)
        {
            var result = await _mediator.Send(new DeleteCourseScheduleCommand
            {
                CourseScheduleId = courseScheduleId
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
                message = "Xóa lịch học thành công."
            });
        }
    }
}