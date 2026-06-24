using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Users.Commands.CreateUser;
using Quanlyhocthem.Application.Features.Users.Commands.DeleteUser;
using Quanlyhocthem.Application.Features.Users.Commands.UpdateUser;
using Quanlyhocthem.Application.Features.Users.Queries.GetStudents;
using Quanlyhocthem.Application.Features.Users.Queries.GetTeachers;
using Quanlyhocthem.Application.Features.Users.Queries.GetUserById;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý tài khoản người dùng
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/users/teachers
        // Admin xem danh sách giảng viên
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await _mediator.Send(new GetTeachersQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách giảng viên thành công.",
                data = result.Data
            });
        }

        // GET: /api/users/students
        // Admin xem danh sách học viên
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var result = await _mediator.Send(new GetStudentsQuery());

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách học viên thành công.",
                data = result.Data
            });
        }

        // GET: /api/users/{userId}
        // Admin xem chi tiết một tài khoản
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery
            {
                UserId = userId
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
                message = "Lấy chi tiết tài khoản thành công.",
                data = result.Data
            });
        }

        // POST: /api/users
        // Admin tạo tài khoản
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
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
                message = "Tạo tài khoản thành công.",
                userId = result.Data
            });
        }

        // PUT: /api/users/{userId}
        // Admin cập nhật tài khoản
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand command)
        {
            command.UserId = userId;

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
                message = "Cập nhật tài khoản thành công."
            });
        }

        // DELETE: /api/users/{userId}
        // Admin xóa tài khoản nếu tài khoản chưa phát sinh dữ liệu quan trọng
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand
            {
                UserId = userId
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
                message = "Xóa tài khoản thành công."
            });
        }
    }
}