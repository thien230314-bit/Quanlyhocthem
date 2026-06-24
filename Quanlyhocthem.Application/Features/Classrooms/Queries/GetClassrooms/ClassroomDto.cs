namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassrooms
{
    // DTO trả danh sách phòng học ra API
    public class ClassroomDto
    {
        // Id phòng học
        public Guid Id { get; set; }

        // Tên phòng học
        public string Name { get; set; } = string.Empty;

        // Mã phòng học
        public string? Code { get; set; }

        // Sức chứa phòng
        public int Capacity { get; set; }

        // Trạng thái hoạt động
        public bool IsActive { get; set; }
    }
}