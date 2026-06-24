using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Submission là bài nộp của học viên cho một bài tập
    public class Submission : BaseEntity
    {
        // Bài tập được nộp
        public Guid AssignmentId { get; private set; }

        public Assignment Assignment { get; private set; } = null!;

        // Enrollment cho biết học viên nào thuộc lớp nào
        public Guid EnrollmentId { get; private set; }

        public Enrollment Enrollment { get; private set; } = null!;

        // Nội dung học viên nhập khi nộp bài
        public string Content { get; private set; } = string.Empty;

        // Tên file bài nộp nếu có
        public string? SubmittedFileName { get; private set; }

        // Đường dẫn file bài nộp nếu có
        public string? SubmittedFileUrl { get; private set; }

        // Thời gian nộp bài
        public DateTime SubmittedAt { get; private set; }

        // Trạng thái bài nộp
        public SubmissionStatus Status { get; private set; }

        // Điểm / nhận xét sau khi chấm
        public Grade? Grade { get; private set; }

        private Submission()
        {
        }

        public Submission(
            Guid assignmentId,
            Guid enrollmentId,
            string content,
            string? submittedFileName,
            string? submittedFileUrl,
            DateTime submittedAt,
            bool isLate)
        {
            AssignmentId = assignmentId;
            EnrollmentId = enrollmentId;
            Content = content?.Trim() ?? string.Empty;
            SubmittedFileName = submittedFileName?.Trim();
            SubmittedFileUrl = submittedFileUrl?.Trim();
            SubmittedAt = submittedAt;
            Status = isLate ? SubmissionStatus.Late : SubmissionStatus.Submitted;
        }

        public void UpdateSubmission(
            string content,
            string? submittedFileName,
            string? submittedFileUrl,
            DateTime submittedAt,
            bool isLate)
        {
            Content = content?.Trim() ?? string.Empty;
            SubmittedFileName = submittedFileName?.Trim();
            SubmittedFileUrl = submittedFileUrl?.Trim();
            SubmittedAt = submittedAt;
            Status = isLate ? SubmissionStatus.Late : SubmissionStatus.Submitted;
            SetUpdatedAt();
        }

        public void MarkAsGraded()
        {
            Status = SubmissionStatus.Graded;
            SetUpdatedAt();
        }
    }
}