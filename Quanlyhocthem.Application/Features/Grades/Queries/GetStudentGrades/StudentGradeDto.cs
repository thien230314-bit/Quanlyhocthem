using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Grades.Queries.GetStudentGrades
{
    // DTO trả điểm và nhận xét bài tập cho học viên
    public class StudentGradeDto
    {
        // Id bài tập
        public Guid AssignmentId { get; set; }

        // Tiêu đề bài tập
        public string AssignmentTitle { get; set; } = string.Empty;

        // Id lớp học
        public Guid CourseId { get; set; }

        // Tên lớp học
        public string CourseName { get; set; } = string.Empty;

        // Tên giảng viên
        public string TeacherName { get; set; } = string.Empty;

        // Id bài nộp
        public Guid SubmissionId { get; set; }

        // Nội dung bài nộp
        public string SubmissionContent { get; set; } = string.Empty;

        // Tên file bài nộp nếu có
        public string? SubmittedFileName { get; set; }

        // Đường dẫn file bài nộp nếu có
        public string? SubmittedFileUrl { get; set; }

        // Thời gian học viên nộp bài
        public DateTime SubmittedAt { get; set; }

        // Trạng thái bài nộp
        public SubmissionStatus SubmissionStatus { get; set; }

        // Điểm tối đa của bài tập
        public decimal MaxScore { get; set; }

        // Điểm giảng viên chấm
        public decimal Score { get; set; }

        // Nhận xét của giảng viên
        public string? Feedback { get; set; }

        // Thời gian chấm điểm
        public DateTime GradedAt { get; set; }
    }
}