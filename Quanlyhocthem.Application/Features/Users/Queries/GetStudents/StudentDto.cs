namespace Quanlyhocthem.Application.Features.Users.Queries.GetStudents
{
    // DTO trả danh sách học sinh cho Admin
    public class StudentDto
    {
        // Id tài khoản User của học sinh
        public Guid UserId { get; set; }

        // Id hồ sơ Student
        public Guid StudentId { get; set; }

        // Mã học viên
        public string StudentCode { get; set; } = string.Empty;

        // Tên đăng nhập
        public string UserName { get; set; } = string.Empty;

        // Họ tên học sinh
        public string FullName { get; set; } = string.Empty;

        // Email
        public string Email { get; set; } = string.Empty;

        // Số điện thoại
        public string? PhoneNumber { get; set; }

        // Ngày sinh
        public DateTime DateOfBirth { get; set; }

        // Id khóa học viên
        public Guid BatchId { get; set; }

        // Tên khóa học viên
        public string BatchName { get; set; } = string.Empty;

        // Tài khoản còn hoạt động không
        public bool IsActive { get; set; }

        // Ngày tạo tài khoản
        public DateTime CreatedAt { get; set; }
    }
}