namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetAssignmentById
{
    // DTO chi tiết bài tập
    public class AssignmentDetailDto
    {
        public Guid AssignmentId { get; set; }

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public Guid TeacherId { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public decimal MaxScore { get; set; }

        public string? AttachmentFileName { get; set; }

        public string? AttachmentUrl { get; set; }

        public bool IsActive { get; set; }

        public int TotalSubmissions { get; set; }

        public int GradedSubmissions { get; set; }

        public int UngradedSubmissions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}