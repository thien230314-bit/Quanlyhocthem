using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourseStudents
{
    // DTO trả danh sách học sinh trong lớp cho giảng viên
    public class TeacherCourseStudentDto
    {
        // Id bản ghi đăng ký lớp
        public Guid EnrollmentId { get; set; }

        // Id hồ sơ học sinh
        public Guid StudentId { get; set; }

        // Id tài khoản học sinh
        public Guid StudentUserId { get; set; }

        // Mã học viên
        public string StudentCode { get; set; } = string.Empty;

        // Tên đăng nhập học sinh
        public string UserName { get; set; } = string.Empty;

        // Họ tên học sinh
        public string FullName { get; set; } = string.Empty;

        // Email học sinh
        public string Email { get; set; } = string.Empty;

        // Số điện thoại học sinh
        public string? PhoneNumber { get; set; }

        // Tên khóa học viên
        public string BatchName { get; set; } = string.Empty;

        // Trạng thái đăng ký lớp
        public EnrollmentStatus EnrollmentStatus { get; set; }

        // Ngày học sinh đăng ký lớp
        public DateTime RegisteredAt { get; set; }
    }
}