using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Queries.GetMyNotifications
{
    // Query để người dùng hiện tại xem thông báo của mình
    public class GetMyNotificationsQuery : IRequest<Result<List<NotificationDto>>>
    {
        // Id User hiện tại, lấy từ JWT token
        public Guid CurrentUserId { get; set; }

        // Nếu true thì chỉ lấy thông báo chưa đọc
        public bool UnreadOnly { get; set; }
    }
}