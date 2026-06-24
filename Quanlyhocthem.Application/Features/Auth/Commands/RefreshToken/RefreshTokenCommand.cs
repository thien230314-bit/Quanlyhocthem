using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Application.Features.Auth.Commands.Login;

namespace Quanlyhocthem.Application.Features.Auth.Commands.RefreshToken
{
    // Command dùng Refresh Token để xin Access Token mới
    public class RefreshTokenCommand : IRequest<Result<LoginResultDto>>
    {
        // Refresh Token cũ client gửi lên
        public string RefreshToken { get; set; } = string.Empty;
    }
}