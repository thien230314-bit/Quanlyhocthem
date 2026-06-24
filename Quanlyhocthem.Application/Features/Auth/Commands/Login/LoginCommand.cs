using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Auth.Commands.Login
{
    // Command đăng nhập
    public class LoginCommand : IRequest<Result<LoginResultDto>>
    {
        // Tên đăng nhập
        public string UserName { get; set; } = string.Empty;

        // Mật khẩu người dùng nhập
        public string Password { get; set; } = string.Empty;
    }
}