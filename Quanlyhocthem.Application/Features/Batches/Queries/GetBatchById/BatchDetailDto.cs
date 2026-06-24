namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatchById
{
    // DTO chi tiết khóa học viên
    public class BatchDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int TotalStudents { get; set; }

        public int TotalCourses { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}