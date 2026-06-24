using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Notification là thông báo gửi đến từng tài khoản trong hệ thống
    public class Notification : BaseEntity
    {
        // Người nhận thông báo
        public Guid ReceiverUserId { get; private set; }

        public User ReceiverUser { get; private set; } = null!;

        // Tiêu đề thông báo
        public string Title { get; private set; } = string.Empty;

        // Nội dung thông báo
        public string Message { get; private set; } = string.Empty;

        // Loại thông báo
        public NotificationType Type { get; private set; }

        // Id dữ liệu liên quan, ví dụ AssignmentId, SubmissionId, GradeId
        public Guid? RelatedEntityId { get; private set; }

        // Tên loại dữ liệu liên quan, ví dụ Assignment, Submission, Grade
        public string? RelatedEntityType { get; private set; }

        // Đã đọc hay chưa
        public bool IsRead { get; private set; }

        // Thời gian đọc thông báo
        public DateTime? ReadAt { get; private set; }

        private Notification()
        {
        }

        public Notification(
            Guid receiverUserId,
            string title,
            string message,
            NotificationType type,
            Guid? relatedEntityId,
            string? relatedEntityType)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Tiêu đề thông báo không được để trống.");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Nội dung thông báo không được để trống.");

            ReceiverUserId = receiverUserId;
            Title = title.Trim();
            Message = message.Trim();
            Type = type;
            RelatedEntityId = relatedEntityId;
            RelatedEntityType = relatedEntityType?.Trim();
            IsRead = false;
        }

        public void MarkAsRead(DateTime readAt)
        {
            if (IsRead)
                return;

            IsRead = true;
            ReadAt = readAt;
            SetUpdatedAt();
        }
    }
}