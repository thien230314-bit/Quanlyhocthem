using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Auth.Commands.Logout
{
    // Command đăng xuất
    public class LogoutCommand : IRequest<Result>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}