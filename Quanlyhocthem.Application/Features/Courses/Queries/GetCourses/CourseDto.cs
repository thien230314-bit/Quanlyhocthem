namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourses
{
    // DTO trả danh sách lớp học ra API
    public class CourseDto
    {
        // Id lớp học
        public Guid Id { get; set; }

        // Tên lớp học
        public string Name { get; set; } = string.Empty;

        // Mô tả lớp học
        public string Description { get; set; } = string.Empty;

        // Học phí
        public decimal TuitionFee { get; set; }

        // Sĩ số tối đa
        public int MaxStudents { get; set; }

        // Số học viên đã đăng ký
        public int CurrentStudents { get; set; }

        // Tên môn học
        public string SubjectName { get; set; } = string.Empty;

        // Tên khóa học viên
        public string BatchName { get; set; } = string.Empty;

        // Tên phòng học
        public string? ClassroomName { get; set; }

        // Tên giảng viên
        public string? TeacherName { get; set; }

        // Ngày bắt đầu
        public DateTime StartDate { get; set; }

        // Ngày kết thúc
        public DateTime? EndDate { get; set; }

        // Trạng thái lớp học
        public bool IsActive { get; set; }
    }
}