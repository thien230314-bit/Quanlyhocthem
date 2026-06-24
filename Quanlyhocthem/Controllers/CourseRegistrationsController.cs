using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.CourseRegistrations.Commands.DeleteCourseRegistration;
using Quanlyhocthem.Application.Features.CourseRegistrations.Commands.RegisterCourse;
using Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetAvailableCourses;
using Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetMyCourses;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller đăng ký lớp học cho học viên
    [ApiController]
    [Route("api/course-registrations")]
    [Authorize(Roles = "Student")]
    public class CourseRegistrationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/course-registrations/available
        // Học viên xem danh sách lớp có thể đăng ký
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCourses()
        {
            var result = await _mediator.Send(new GetAvailableCoursesQuery
            {
                StudentUserId = GetCurrentUserId()
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
                message = "Lấy danh sách lớp có thể đăng ký thành công.",
                data = result.Data
            });
        }

        // GET: /api/course-registrations/my-courses
        // Học viên xem danh sách lớp đã đăng ký
        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var result = await _mediator.Send(new GetMyCoursesQuery
            {
                StudentUserId = GetCurrentUserId()
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
                message = "Lấy danh sách lớp đã đăng ký thành công.",
                data = result.Data
            });
        }

        // POST: /api/course-registrations/register
        // Học viên đăng ký lớp học
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCourse([FromBody] RegisterCourseCommand command)
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
                message = "Đăng ký lớp học thành công.",
                enrollmentId = result.Data
            });
        }

        // DELETE: /api/course-registrations/{courseId}
        // Học viên hủy đăng ký lớp học nếu chưa phát sinh điểm danh, bài nộp, học phí
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseRegistration(Guid courseId)
        {
            var result = await _mediator.Send(new DeleteCourseRegistrationCommand
            {
                StudentUserId = GetCurrentUserId(),
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
                message = "Hủy đăng ký lớp học thành công."
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