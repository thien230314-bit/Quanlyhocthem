using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Auth.Commands.Login;
using Quanlyhocthem.Application.Features.Auth.Commands.Logout;
using Quanlyhocthem.Application.Features.Auth.Commands.RefreshToken;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller xác thực tài khoản
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: /api/auth/login
        // Đăng nhập và nhận Access Token + Refresh Token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
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
                message = "Đăng nhập thành công.",
                data = result.Data
            });
        }

        // POST: /api/auth/refresh-token
        // Dùng Refresh Token để cấp lại Access Token mới
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
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
                message = "Làm mới token thành công.",
                data = result.Data
            });
        }

        // POST: /api/auth/logout
        // Đăng xuất và thu hồi Refresh Token
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
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
                message = "Đăng xuất thành công."
            });
        }
    }
}