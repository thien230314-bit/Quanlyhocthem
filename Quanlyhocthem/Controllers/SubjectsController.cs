using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Subjects.Commands.CreateSubject;
using Quanlyhocthem.Application.Features.Subjects.Commands.DeleteSubject;
using Quanlyhocthem.Application.Features.Subjects.Commands.UpdateSubject;
using Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjectById;
using Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjects;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý môn học
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/subjects
        // Admin xem danh sách môn học
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var result = await _mediator.Send(new GetSubjectsQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách môn học thành công.",
                data = result.Data
            });
        }

        // GET: /api/subjects/{subjectId}
        // Admin xem chi tiết một môn học
        [HttpGet("{subjectId}")]
        public async Task<IActionResult> GetSubjectById(Guid subjectId)
        {
            var result = await _mediator.Send(new GetSubjectByIdQuery
            {
                SubjectId = subjectId
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
                message = "Lấy chi tiết môn học thành công.",
                data = result.Data
            });
        }

        // POST: /api/subjects
        // Admin tạo môn học
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectCommand command)
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
                message = "Tạo môn học thành công.",
                subjectId = result.Data
            });
        }

        // PUT: /api/subjects/{subjectId}
        // Admin cập nhật môn học
        [HttpPut("{subjectId}")]
        public async Task<IActionResult> UpdateSubject(Guid subjectId, [FromBody] UpdateSubjectCommand command)
        {
            command.SubjectId = subjectId;

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
                message = "Cập nhật môn học thành công."
            });
        }

        // DELETE: /api/subjects/{subjectId}
        // Admin xóa môn học nếu chưa được dùng trong lớp học
        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> DeleteSubject(Guid subjectId)
        {
            var result = await _mediator.Send(new DeleteSubjectCommand
            {
                SubjectId = subjectId
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
                message = "Xóa môn học thành công."
            });
        }
    }
}