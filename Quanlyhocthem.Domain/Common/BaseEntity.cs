namespace Quanlyhocthem.Domain.Common
{
    // Lớp cha dùng chung cho tất cả entity trong hệ thống
    // Mục đích: entity nào cũng có Id, ngày tạo, ngày cập nhật
    public abstract class BaseEntity
    {
        // Khóa chính của bảng, dùng Guid để tránh trùng dữ liệu
        public Guid Id { get; protected set; } = Guid.NewGuid();

        // Ngày tạo bản ghi
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        // Ngày cập nhật gần nhất, có thể null nếu chưa từng sửa
        public DateTime? UpdatedAt { get; protected set; }

        // Gọi hàm này mỗi khi entity bị chỉnh sửa
        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}