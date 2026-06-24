using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Assignments.Commands.CreateAssignment;
using Quanlyhocthem.Application.Features.Assignments.Commands.DeleteAssignment;
using Quanlyhocthem.Application.Features.Assignments.Commands.UpdateAssignment;
using Quanlyhocthem.Application.Features.Assignments.Queries.GetAssignmentById;
using Quanlyhocthem.Application.Features.Assignments.Queries.GetTeacherCourseAssignments;
using Quanlyhocthem.Application.Features.Grades.Commands.GradeSubmission;
using Quanlyhocthem.Application.Features.Submissions.Queries.GetAssignmentSubmissions;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller bài tập cho giảng viên
    [ApiController]
    [Route("api/teacher/assignments")]
    [Authorize(Roles = "Teacher")]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherAssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/teacher/assignments/{assignmentId}
        // Giảng viên xem chi tiết một bài tập
        [HttpGet("{assignmentId}")]
        public async Task<IActionResult> GetAssignmentById(Guid assignmentId)
        {
            var result = await _mediator.Send(new GetAssignmentByIdQuery
            {
                CurrentUserId = GetCurrentUserId(),
                AssignmentId = assignmentId
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
                message = "Lấy chi tiết bài tập thành công.",
                data = result.Data
            });
        }

        // GET: /api/teacher/assignments/course/{courseId}
        // Giảng viên xem danh sách bài tập đã giao trong một lớp
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseAssignments(Guid courseId)
        {
            var result = await _mediator.Send(new GetTeacherCourseAssignmentsQuery
            {
                TeacherUserId = GetCurrentUserId(),
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
                message = "Lấy danh sách bài tập của lớp thành công.",
                data = result.Data
            });
        }

        // GET: /api/teacher/assignments/{assignmentId}/submissions
        // Giảng viên xem danh sách bài nộp của một bài tập
        [HttpGet("{assignmentId}/submissions")]
        public async Task<IActionResult> GetAssignmentSubmissions(Guid assignmentId)
        {
            var result = await _mediator.Send(new GetAssignmentSubmissionsQuery
            {
                TeacherUserId = GetCurrentUserId(),
                AssignmentId = assignmentId
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
                message = "Lấy danh sách bài nộp thành công.",
                data = result.Data
            });
        }

        // POST: /api/teacher/assignments
        // Giảng viên tạo bài tập cho lớp mình phụ trách
        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentCommand command)
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
                message = "Tạo bài tập thành công.",
                assignmentId = result.Data
            });
        }

        // PUT: /api/teacher/assignments/{assignmentId}
        // Giảng viên cập nhật bài tập đã giao
        [HttpPut("{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment(Guid assignmentId, [FromBody] UpdateAssignmentCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();
            command.AssignmentId = assignmentId;

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
                message = "Cập nhật bài tập thành công."
            });
        }

        // DELETE: /api/teacher/assignments/{assignmentId}
        // Giảng viên xóa bài tập nếu chưa có học viên nộp bài
        [HttpDelete("{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
        {
            var result = await _mediator.Send(new DeleteAssignmentCommand
            {
                TeacherUserId = GetCurrentUserId(),
                AssignmentId = assignmentId
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
                message = "Xóa bài tập thành công."
            });
        }

        // POST: /api/teacher/assignments/submissions/{submissionId}/grade
        // Giảng viên chấm điểm bài nộp
        [HttpPost("submissions/{submissionId}/grade")]
        public async Task<IActionResult> GradeSubmission(Guid submissionId, [FromBody] GradeSubmissionCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();
            command.SubmissionId = submissionId;

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
                message = "Chấm điểm bài nộp thành công.",
                gradeId = result.Data
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