using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Submissions.Queries.GetAssignmentSubmissions
{
    // DTO trả danh sách bài nộp cho giảng viên
    public class AssignmentSubmissionDto
    {
        // Id bài nộp
        public Guid SubmissionId { get; set; }

        // Id bài tập
        public Guid AssignmentId { get; set; }

        // Tiêu đề bài tập
        public string AssignmentTitle { get; set; } = string.Empty;

        // Id học viên
        public Guid StudentId { get; set; }

        // Mã học viên
        public string StudentCode { get; set; } = string.Empty;

        // Họ tên học viên
        public string StudentName { get; set; } = string.Empty;

        // Email học viên
        public string StudentEmail { get; set; } = string.Empty;

        // Nội dung bài nộp
        public string Content { get; set; } = string.Empty;

        // Tên file bài nộp
        public string? SubmittedFileName { get; set; }

        // Đường dẫn file bài nộp
        public string? SubmittedFileUrl { get; set; }

        // Thời gian nộp
        public DateTime SubmittedAt { get; set; }

        // Trạng thái bài nộp
        public SubmissionStatus Status { get; set; }

        // Đã được chấm chưa
        public bool IsGraded { get; set; }

        // Điểm nếu đã chấm
        public decimal? Score { get; set; }

        // Nhận xét nếu đã chấm
        public string? Feedback { get; set; }

        // Thời gian chấm nếu đã chấm
        public DateTime? GradedAt { get; set; }
    }
}