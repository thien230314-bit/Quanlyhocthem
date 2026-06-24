using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    // Handler đánh dấu thông báo đã đọc
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result>
    {
        private readonly IAppDbContext _context;

        public MarkNotificationAsReadCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            // Tìm đúng thông báo của học viên hiện tại
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.Id == request.NotificationId &&
                    x.ReceiverUserId == request.StudentUserId,
                    cancellationToken);

            if (notification == null)
                return Result.Failure("Không tìm thấy thông báo.");

            notification.MarkAsRead(DateTime.UtcNow);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}