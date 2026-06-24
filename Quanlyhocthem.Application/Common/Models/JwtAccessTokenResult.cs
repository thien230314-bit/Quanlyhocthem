namespace Quanlyhocthem.Application.Common.Models
{
    // Kết quả tạo Access Token kèm thời gian hết hạn
    public class JwtAccessTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; set; }
    }
}