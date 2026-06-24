using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    // Command để học viên đánh dấu thông báo đã đọc
    public class MarkNotificationAsReadCommand : IRequest<Result>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id thông báo cần đánh dấu đã đọc
        public Guid NotificationId { get; set; }
    }
}