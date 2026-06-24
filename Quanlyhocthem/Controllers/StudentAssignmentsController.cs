using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Assignments.Queries.GetAssignmentById;
using Quanlyhocthem.Application.Features.Assignments.Queries.GetStudentAssignments;
using Quanlyhocthem.Application.Features.Grades.Queries.GetStudentGrades;
using Quanlyhocthem.Application.Features.Submissions.Commands.SubmitAssignment;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller bài tập cho học viên
    [ApiController]
    [Route("api/student/assignments")]
    [Authorize(Roles = "Student")]
    public class StudentAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentAssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/student/assignments
        // Học viên xem danh sách bài tập trong các lớp đã đăng ký
        [HttpGet]
        public async Task<IActionResult> GetMyAssignments()
        {
            var result = await _mediator.Send(new GetStudentAssignmentsQuery
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
                message = "Lấy danh sách bài tập thành công.",
                data = result.Data
            });
        }

        // GET: /api/student/assignments/grades
        // Học viên xem điểm và nhận xét các bài đã được chấm
        [HttpGet("grades")]
        public async Task<IActionResult> GetMyGrades()
        {
            var result = await _mediator.Send(new GetStudentGradesQuery
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
                message = "Lấy danh sách điểm thành công.",
                data = result.Data
            });
        }

        // GET: /api/student/assignments/{assignmentId}
        // Học viên xem chi tiết một bài tập
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

        // POST: /api/student/assignments/submit
        // Học viên nộp bài tập
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAssignment([FromBody] SubmitAssignmentCommand command)
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
                message = "Nộp bài thành công.",
                submissionId = result.Data
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