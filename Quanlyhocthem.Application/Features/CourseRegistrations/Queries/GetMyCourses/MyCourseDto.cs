using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetMyCourses
{
    // DTO hiển thị khóa học học viên đã đăng ký
    public class MyCourseDto
    {
        // Id đăng ký
        public Guid EnrollmentId { get; set; }

        // Id khóa học
        public Guid CourseId { get; set; }

        // Tên khóa học
        public string CourseName { get; set; } = string.Empty;

        // Tên giảng viên
        public string? TeacherName { get; set; }

        // Ngày đăng ký
        public DateTime EnrollmentDate { get; set; }

        // Trạng thái đăng ký
        public EnrollmentStatus Status { get; set; }
    }
}