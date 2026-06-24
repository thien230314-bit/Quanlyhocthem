using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetStudentAssignments
{
    // DTO trả danh sách bài tập cho học viên
    public class StudentAssignmentDto
    {
        // Id bài tập
        public Guid AssignmentId { get; set; }

        // Id lớp học
        public Guid CourseId { get; set; }

        // Tên lớp học
        public string CourseName { get; set; } = string.Empty;

        // Tên giảng viên giao bài
        public string TeacherName { get; set; } = string.Empty;

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

        // Học viên đã nộp bài chưa
        public bool IsSubmitted { get; set; }

        // Bài tập đã quá hạn chưa
        public bool IsOverdue { get; set; }

        // Id bài nộp nếu đã nộp
        public Guid? SubmissionId { get; set; }

        // Thời gian nộp nếu đã nộp
        public DateTime? SubmittedAt { get; set; }

        // Trạng thái bài nộp nếu đã nộp
        public SubmissionStatus? SubmissionStatus { get; set; }
    }
}