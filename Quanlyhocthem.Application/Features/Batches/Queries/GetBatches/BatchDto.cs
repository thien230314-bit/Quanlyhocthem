namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatches
{
    // DTO trả danh sách khóa học viên ra API
    public class BatchDto
    {
        // Id khóa học viên
        public Guid Id { get; set; }

        // Tên khóa học viên
        public string Name { get; set; } = string.Empty;

        // Mô tả khóa
        public string? Description { get; set; }

        // Trạng thái hoạt động
        public bool IsActive { get; set; }
    }
}