using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Common.Interfaces
{
    // Service tạo JWT Access Token và Refresh Token
    public interface IJwtTokenService
    {
        // Hàm cũ để tránh vỡ code những chỗ đang gọi GenerateToken
        string GenerateToken(User user);

        // Tạo Access Token kèm thời gian hết hạn
        JwtAccessTokenResult GenerateAccessToken(User user);

        // Tạo chuỗi Refresh Token ngẫu nhiên
        string GenerateRefreshToken();

        // Lấy thời gian hết hạn của Refresh Token
        DateTime GetRefreshTokenExpiresAt();
    }
}