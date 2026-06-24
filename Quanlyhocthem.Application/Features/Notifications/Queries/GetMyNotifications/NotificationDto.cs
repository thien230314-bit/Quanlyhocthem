using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Notifications.Queries.GetMyNotifications
{
    // DTO trả thông báo cho người dùng hiện tại
    public class NotificationDto
    {
        // Id thông báo
        public Guid NotificationId { get; set; }

        // Tiêu đề thông báo
        public string Title { get; set; } = string.Empty;

        // Nội dung thông báo
        public string Message { get; set; } = string.Empty;

        // Loại thông báo
        public NotificationType Type { get; set; }

        // Id dữ liệu liên quan
        public Guid? RelatedEntityId { get; set; }

        // Loại dữ liệu liên quan
        public string? RelatedEntityType { get; set; }

        // Đã đọc chưa
        public bool IsRead { get; set; }

        // Thời gian đọc
        public DateTime? ReadAt { get; set; }

        // Thời gian tạo
        public DateTime CreatedAt { get; set; }
    }
}