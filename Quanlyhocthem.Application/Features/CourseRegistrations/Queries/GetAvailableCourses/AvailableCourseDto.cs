namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetAvailableCourses
{
    // DTO hiển thị khóa học học viên có thể đăng ký
    public class AvailableCourseDto
    {
        // Id khóa học
        public Guid Id { get; set; }

        // Tên khóa học
        public string Name { get; set; } = string.Empty;

        // Mô tả khóa học
        public string Description { get; set; } = string.Empty;

        // Học phí
        public decimal TuitionFee { get; set; }

        // Sĩ số tối đa
        public int MaxStudents { get; set; }

        // Số học viên đã đăng ký
        public int CurrentStudents { get; set; }

        // Tên giảng viên
        public string? TeacherName { get; set; }
    }
}