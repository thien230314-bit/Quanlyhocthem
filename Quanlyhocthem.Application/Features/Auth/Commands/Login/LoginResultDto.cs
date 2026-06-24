using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Auth.Commands.Login
{
    // Dữ liệu trả về sau khi đăng nhập thành công
    public class LoginResultDto
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; }

        // JWT Access Token dùng để gọi API
        public string AccessToken { get; set; } = string.Empty;

        // Refresh Token dùng để xin Access Token mới khi Access Token hết hạn
        public string RefreshToken { get; set; } = string.Empty;

        // Thời gian hết hạn Access Token
        public DateTime AccessTokenExpiresAt { get; set; }

        // Thời gian hết hạn Refresh Token
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}