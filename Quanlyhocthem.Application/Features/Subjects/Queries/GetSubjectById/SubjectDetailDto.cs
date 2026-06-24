namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjectById
{
    // DTO chi tiết môn học
    public class SubjectDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}