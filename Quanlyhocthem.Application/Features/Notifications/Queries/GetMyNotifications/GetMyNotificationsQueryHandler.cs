using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Queries.GetMyNotifications
{
    // Handler lấy thông báo của người dùng hiện tại
    public class GetMyNotificationsQueryHandler : IRequestHandler<GetMyNotificationsQuery, Result<List<NotificationDto>>>
    {
        private readonly IAppDbContext _context;

        public GetMyNotificationsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<NotificationDto>>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra user có tồn tại không
            var userExists = await _context.Users
                .AnyAsync(x => x.Id == request.CurrentUserId, cancellationToken);

            if (!userExists)
                return Result<List<NotificationDto>>.Failure("Không tìm thấy tài khoản người dùng.");

            // Lấy thông báo theo người nhận
            var query = _context.Notifications
                .Where(x => x.ReceiverUserId == request.CurrentUserId);

            // Lọc thông báo chưa đọc nếu có yêu cầu
            if (request.UnreadOnly)
            {
                query = query.Where(x => !x.IsRead);
            }

            var notifications = await query
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedAt)
                .Select(x => new NotificationDto
                {
                    NotificationId = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    Type = x.Type,
                    RelatedEntityId = x.RelatedEntityId,
                    RelatedEntityType = x.RelatedEntityType,
                    IsRead = x.IsRead,
                    ReadAt = x.ReadAt,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<NotificationDto>>.Success(notifications);
        }
    }
}