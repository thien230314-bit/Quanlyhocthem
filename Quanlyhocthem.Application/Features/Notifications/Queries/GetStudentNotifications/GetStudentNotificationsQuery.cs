using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Queries.GetStudentNotifications
{
    // Query để học viên xem thông báo của mình
    public class GetStudentNotificationsQuery : IRequest<Result<List<StudentNotificationDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Nếu true thì chỉ lấy thông báo chưa đọc
        public bool UnreadOnly { get; set; }
    }
}