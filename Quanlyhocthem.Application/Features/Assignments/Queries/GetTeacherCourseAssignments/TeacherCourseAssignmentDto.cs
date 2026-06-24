namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetTeacherCourseAssignments
{
    // DTO trả danh sách bài tập giảng viên đã giao trong một lớp
    public class TeacherCourseAssignmentDto
    {
        // Id bài tập
        public Guid AssignmentId { get; set; }

        // Id lớp học
        public Guid CourseId { get; set; }

        // Tên lớp học
        public string CourseName { get; set; } = string.Empty;

        // Tiêu đề bài tập
        public string Title { get; set; } = string.Empty;

        // Mô tả bài tập
        public string Description { get; set; } = string.Empty;

        // Hạn nộp bài
        public DateTime DueDate { get; set; }

        // Điểm tối đa
        public decimal MaxScore { get; set; }

        // Tên file đính kèm nếu có
        public string? AttachmentFileName { get; set; }

        // Đường dẫn file đính kèm nếu có
        public string? AttachmentUrl { get; set; }

        // Bài tập còn hoạt động không
        public bool IsActive { get; set; }

        // Thời gian tạo bài tập
        public DateTime CreatedAt { get; set; }

        // Tổng số bài nộp
        public int TotalSubmissions { get; set; }

        // Số bài đã chấm
        public int GradedSubmissions { get; set; }

        // Số bài chưa chấm
        public int UngradedSubmissions { get; set; }
    }
}