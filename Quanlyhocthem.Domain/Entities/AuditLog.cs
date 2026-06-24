using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Entity AuditLog lưu nhật ký thao tác quan trọng trong hệ thống
    public class AuditLog : BaseEntity
    {
        // Hành động đã thực hiện, ví dụ: CreateUser, DeleteCourse
        public string Action { get; private set; } = string.Empty;

        // Tên entity bị tác động, ví dụ: User, Course
        public string EntityName { get; private set; } = string.Empty;

        // Id của entity bị tác động
        public Guid? EntityId { get; private set; }

        // Id người thực hiện hành động
        public Guid? UserId { get; private set; }

        // Thời điểm ghi log
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        // Constructor rỗng cho Entity Framework Core
        private AuditLog()
        {
        }

        // Constructor tạo log
        public AuditLog(string action, string entityName, Guid? entityId, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Tên hành động không được để trống.");

            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentException("Tên entity không được để trống.");

            Action = action.Trim();
            EntityName = entityName.Trim();
            EntityId = entityId;
            UserId = userId;
            CreatedOn = DateTime.UtcNow;
        }
    }
}