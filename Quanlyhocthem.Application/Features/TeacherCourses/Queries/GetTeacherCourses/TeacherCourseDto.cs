namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourses
{
    // DTO trả danh sách lớp học được gán cho giảng viên
    public class TeacherCourseDto
    {
        // Id lớp học
        public Guid CourseId { get; set; }

        // Tên lớp học
        public string CourseName { get; set; } = string.Empty;

        // Mô tả lớp học
        public string Description { get; set; } = string.Empty;

        // Tên môn học
        public string SubjectName { get; set; } = string.Empty;

        // Tên khóa học viên
        public string BatchName { get; set; } = string.Empty;

        // Tên phòng học
        public string? ClassroomName { get; set; }

        // Học phí
        public decimal TuitionFee { get; set; }

        // Sĩ số tối đa
        public int MaxStudents { get; set; }

        // Số học viên hiện tại
        public int CurrentStudents { get; set; }

        // Ngày bắt đầu lớp
        public DateTime StartDate { get; set; }

        // Ngày kết thúc lớp
        public DateTime? EndDate { get; set; }

        // Lớp còn hoạt động không
        public bool IsActive { get; set; }
    }
}