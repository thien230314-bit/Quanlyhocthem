namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassroomById
{
    // DTO chi tiết phòng học
    public class ClassroomDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}