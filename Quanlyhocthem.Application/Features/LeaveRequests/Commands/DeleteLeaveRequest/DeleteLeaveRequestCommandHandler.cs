using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest
{
    // Handler xử lý học viên xóa đơn xin nghỉ
    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.LeaveRequestId == Guid.Empty)
                return Result.Failure("Id đơn xin nghỉ không hợp lệ.");

            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result.Failure("Không tìm thấy hồ sơ học viên.");

            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(x =>
                    x.Id == request.LeaveRequestId &&
                    x.StudentId == student.Id,
                    cancellationToken);

            if (leaveRequest == null)
                return Result.Failure("Không tìm thấy đơn xin nghỉ.");

            if (leaveRequest.Status != LeaveRequestStatus.PendingTeacher)
                return Result.Failure("Chỉ được xóa đơn khi đang chờ giảng viên duyệt.");

            var notifications = await _context.Notifications
                .Where(x =>
                    x.RelatedEntityId == request.LeaveRequestId &&
                    x.RelatedEntityType == "LeaveRequest")
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
            }

            _context.LeaveRequests.Remove(leaveRequest);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}