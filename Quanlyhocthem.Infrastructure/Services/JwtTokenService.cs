using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Quanlyhocthem.Infrastructure.Services
{
    // Service tạo JWT Access Token và Refresh Token
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hàm cũ để tránh vỡ code những chỗ đang gọi GenerateToken
        public string GenerateToken(User user)
        {
            return GenerateAccessToken(user).AccessToken;
        }

        // Tạo Access Token kèm thời gian hết hạn
        public JwtAccessTokenResult GenerateAccessToken(User user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("Jwt:SecretKey chưa được cấu hình.");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // Đọc thời gian hết hạn Access Token từ appsettings
            // Nếu không có cấu hình thì mặc định là 30 phút
            var accessTokenExpirationMinutes = GetIntConfigValue(
                "Jwt:AccessTokenExpirationMinutes",
                30
            );

            var expiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims là thông tin được nhúng vào Access Token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("fullName", user.FullName),
                new Claim("email", user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwtAccessTokenResult
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = expiresAt
            };
        }

        // Tạo Refresh Token ngẫu nhiên, khó đoán
        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        // Lấy thời gian hết hạn Refresh Token
        public DateTime GetRefreshTokenExpiresAt()
        {
            // Đọc thời gian hết hạn Refresh Token từ appsettings
            // Nếu không có cấu hình thì mặc định là 7 ngày
            var refreshTokenExpirationDays = GetIntConfigValue(
                "Jwt:RefreshTokenExpirationDays",
                7
            );

            return DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
        }

        // Đọc số nguyên từ appsettings, nếu không đọc được thì dùng giá trị mặc định
        private int GetIntConfigValue(string key, int defaultValue)
        {
            var value = _configuration[key];

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (!int.TryParse(value, out var result))
                return defaultValue;

            return result;
        }
    }
}