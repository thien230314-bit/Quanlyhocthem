using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Classrooms.Commands.CreateClassroom;
using Quanlyhocthem.Application.Features.Classrooms.Commands.DeleteClassroom;
using Quanlyhocthem.Application.Features.Classrooms.Commands.UpdateClassroom;
using Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassroomById;
using Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassrooms;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý phòng học
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ClassroomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassroomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/classrooms
        // Admin xem danh sách phòng học
        [HttpGet]
        public async Task<IActionResult> GetClassrooms()
        {
            var result = await _mediator.Send(new GetClassroomsQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách phòng học thành công.",
                data = result.Data
            });
        }

        // GET: /api/classrooms/{classroomId}
        // Admin xem chi tiết một phòng học
        [HttpGet("{classroomId}")]
        public async Task<IActionResult> GetClassroomById(Guid classroomId)
        {
            var result = await _mediator.Send(new GetClassroomByIdQuery
            {
                ClassroomId = classroomId
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
                message = "Lấy chi tiết phòng học thành công.",
                data = result.Data
            });
        }

        // POST: /api/classrooms
        // Admin tạo phòng học
        [HttpPost]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomCommand command)
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
                message = "Tạo phòng học thành công.",
                classroomId = result.Data
            });
        }

        // PUT: /api/classrooms/{classroomId}
        // Admin cập nhật phòng học
        [HttpPut("{classroomId}")]
        public async Task<IActionResult> UpdateClassroom(Guid classroomId, [FromBody] UpdateClassroomCommand command)
        {
            command.ClassroomId = classroomId;

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
                message = "Cập nhật phòng học thành công."
            });
        }

        // DELETE: /api/classrooms/{classroomId}
        // Admin xóa phòng học nếu phòng chưa được lớp nào sử dụng
        [HttpDelete("{classroomId}")]
        public async Task<IActionResult> DeleteClassroom(Guid classroomId)
        {
            var result = await _mediator.Send(new DeleteClassroomCommand
            {
                ClassroomId = classroomId
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
                message = "Xóa phòng học thành công."
            });
        }
    }
}