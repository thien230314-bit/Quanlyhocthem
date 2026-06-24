using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Courses.Commands.CreateCourse;
using Quanlyhocthem.Application.Features.Courses.Commands.DeleteCourse;
using Quanlyhocthem.Application.Features.Courses.Commands.UpdateCourse;
using Quanlyhocthem.Application.Features.Courses.Queries.GetCourseById;
using Quanlyhocthem.Application.Features.Courses.Queries.GetCourses;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý lớp học / khóa học
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/courses
        // Admin xem danh sách lớp học
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var result = await _mediator.Send(new GetCoursesQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách lớp học thành công.",
                data = result.Data
            });
        }

        // GET: /api/courses/{courseId}
        // Admin xem chi tiết một lớp học
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            var result = await _mediator.Send(new GetCourseByIdQuery
            {
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
                message = "Lấy chi tiết lớp học thành công.",
                data = result.Data
            });
        }

        // POST: /api/courses
        // Admin tạo lớp học cụ thể
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
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
                message = "Tạo lớp học thành công.",
                courseId = result.Data
            });
        }

        // PUT: /api/courses/{courseId}
        // Admin cập nhật lớp học
        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(Guid courseId, [FromBody] UpdateCourseCommand command)
        {
            command.CourseId = courseId;

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
                message = "Cập nhật lớp học thành công."
            });
        }

        // DELETE: /api/courses/{courseId}
        // Admin xóa lớp học nếu chưa phát sinh dữ liệu
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var result = await _mediator.Send(new DeleteCourseCommand
            {
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
                message = "Xóa lớp học thành công."
            });
        }
    }
}