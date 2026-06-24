using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Entity User dùng chung cho cả 3 role: Admin, Teacher, Student
    public class User : BaseEntity
    {
        // Tên đăng nhập dùng khi login
        public string UserName { get; private set; } = string.Empty;

        // Mật khẩu. Hiện tại đặt tên PasswordHash để sau này có thể mã hóa
        public string PasswordHash { get; private set; } = string.Empty;

        // Họ tên đầy đủ của người dùng
        public string FullName { get; private set; } = string.Empty;

        // Email dùng để liên hệ hoặc đăng nhập nếu cần
        public string Email { get; private set; } = string.Empty;

        // Số điện thoại, có thể không nhập
        public string? PhoneNumber { get; private set; }

        // Vai trò trong hệ thống: Admin, Teacher, Student
        public Role Role { get; private set; }

        // Tài khoản còn hoạt động hay bị khóa
        public bool IsActive { get; private set; } = true;

        // RefreshToken dùng nếu sau này làm JWT refresh token
        public string? RefreshToken { get; private set; }

        // Thời gian hết hạn refresh token
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        // Constructor rỗng cho Entity Framework Core
        private User()
        {
        }

        // Constructor chính để tạo user mới
        public User(string userName, string passwordHash, string fullName, string email, string? phoneNumber, Role role)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Tên đăng nhập không được để trống.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Mật khẩu không được để trống.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được để trống.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống.");

            UserName = userName.Trim();
            PasswordHash = passwordHash;
            FullName = fullName.Trim();
            Email = email.Trim();
            PhoneNumber = phoneNumber;
            Role = role;
            IsActive = true;
        }


        // Cập nhật thông tin cá nhân
        public void UpdateProfile(string fullName, string email, string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được để trống.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống.");

            FullName = fullName.Trim();
            Email = email.Trim();
            PhoneNumber = phoneNumber;
            SetUpdatedAt();
        }

        // Đổi mật khẩu
        public void ChangePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Mật khẩu không được để trống.");

            PasswordHash = passwordHash;
            SetUpdatedAt();
        }

        // Khóa tài khoản
        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }

        // Mở khóa tài khoản
        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }

        // Cập nhật refresh token
        public void UpdateRefreshToken(string refreshToken, DateTime expiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
            SetUpdatedAt();
        }

        // Xóa refresh token khi logout
        public void RevokeRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
            SetUpdatedAt();
        }
    }
}