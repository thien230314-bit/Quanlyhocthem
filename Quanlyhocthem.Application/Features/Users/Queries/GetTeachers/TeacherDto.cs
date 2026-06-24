namespace Quanlyhocthem.Application.Features.Users.Queries.GetTeachers
{
    // DTO trả danh sách giảng viên cho Admin
    public class TeacherDto
    {
        // Id tài khoản giảng viên
        public Guid Id { get; set; }

        // Tên đăng nhập
        public string UserName { get; set; } = string.Empty;

        // Họ tên giảng viên
        public string FullName { get; set; } = string.Empty;

        // Email
        public string Email { get; set; } = string.Empty;

        // Số điện thoại
        public string? PhoneNumber { get; set; }

        // Tài khoản còn hoạt động không
        public bool IsActive { get; set; }

        // Ngày tạo tài khoản
        public DateTime CreatedAt { get; set; }
    }
}