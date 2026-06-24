using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Queries.GetStudentNotifications
{
    // Handler lấy danh sách thông báo của học viên
    public class GetStudentNotificationsQueryHandler : IRequestHandler<GetStudentNotificationsQuery, Result<List<StudentNotificationDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentNotificationsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentNotificationDto>>> Handle(GetStudentNotificationsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra hồ sơ học viên có tồn tại không
            var studentExists = await _context.Students
                .AnyAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (!studentExists)
                return Result<List<StudentNotificationDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            // Lấy thông báo theo UserId
            var query = _context.Notifications
                .Where(x => x.ReceiverUserId == request.StudentUserId);

            // Nếu chỉ lấy chưa đọc
            if (request.UnreadOnly)
            {
                query = query.Where(x => !x.IsRead);
            }

            var notifications = await query
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedAt)
                .Select(x => new StudentNotificationDto
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

            return Result<List<StudentNotificationDto>>.Success(notifications);
        }
    }
}