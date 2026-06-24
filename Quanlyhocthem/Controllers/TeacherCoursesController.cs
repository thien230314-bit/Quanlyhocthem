using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourses;
using Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourseStudents;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller cho giảng viên xem lớp được giao và học sinh trong lớp
    [ApiController]
    [Route("api/teacher/courses")]
    [Authorize(Roles = "Teacher")]
    public class TeacherCoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherCoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/teacher/courses
        // Giảng viên xem danh sách lớp được Admin phân công
        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            var teacherUserId = GetCurrentUserId();

            var result = await _mediator.Send(new GetTeacherCoursesQuery
            {
                TeacherUserId = teacherUserId
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
                message = "Lấy danh sách lớp được phân công thành công.",
                data = result.Data
            });
        }

        // GET: /api/teacher/courses/{courseId}/students
        // Giảng viên xem danh sách học sinh trong lớp mình phụ trách
        [HttpGet("{courseId}/students")]
        public async Task<IActionResult> GetCourseStudents(Guid courseId)
        {
            var teacherUserId = GetCurrentUserId();

            var result = await _mediator.Send(new GetTeacherCourseStudentsQuery
            {
                TeacherUserId = teacherUserId,
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
                message = "Lấy danh sách học sinh trong lớp thành công.",
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