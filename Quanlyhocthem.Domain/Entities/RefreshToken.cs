using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // RefreshToken dùng để cấp lại AccessToken khi AccessToken hết hạn
    public class RefreshToken : BaseEntity
    {
        // User sở hữu refresh token này
        public Guid UserId { get; private set; }

        public User User { get; private set; } = null!;

        // Chuỗi refresh token
        public string Token { get; private set; } = string.Empty;

        // Thời gian hết hạn refresh token
        public DateTime ExpiresAt { get; private set; }

        // Thời gian bị thu hồi, null nghĩa là chưa bị thu hồi
        public DateTime? RevokedAt { get; private set; }

        // Token mới thay thế token cũ nếu có xoay vòng refresh token
        public string? ReplacedByToken { get; private set; }

        // Token đã bị thu hồi chưa
        public bool IsRevoked => RevokedAt.HasValue;

        // Token đã hết hạn chưa
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        // Token còn dùng được không
        public bool IsActive => !IsRevoked && !IsExpired;

        private RefreshToken()
        {
        }

        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Id người dùng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Refresh token không được để trống.");

            if (expiresAt <= DateTime.UtcNow)
                throw new ArgumentException("Thời gian hết hạn refresh token không hợp lệ.");

            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }

        // Thu hồi refresh token
        public void Revoke(DateTime revokedAt, string? replacedByToken = null)
        {
            if (IsRevoked)
                throw new InvalidOperationException("Refresh token đã bị thu hồi.");

            RevokedAt = revokedAt;
            ReplacedByToken = string.IsNullOrWhiteSpace(replacedByToken)
                ? null
                : replacedByToken.Trim();

            SetUpdatedAt();
        }
    }
}