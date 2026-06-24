using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.AdminApproveLeaveRequest
{
    // Handler xử lý Admin xác nhận đơn xin nghỉ
    public class AdminApproveLeaveRequestCommandHandler : IRequestHandler<AdminApproveLeaveRequestCommand, Result>
    {
        private readonly IAppDbContext _context;

        public AdminApproveLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AdminApproveLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.LeaveRequestId == Guid.Empty)
                return Result.Failure("Id đơn xin nghỉ không hợp lệ.");

            var admin = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.AdminUserId, cancellationToken);

            if (admin == null)
                return Result.Failure("Không tìm thấy tài khoản Admin.");

            if (admin.Role != Role.Admin)
                return Result.Failure("Tài khoản hiện tại không phải Admin.");

            if (!admin.IsActive)
                return Result.Failure("Tài khoản Admin đang bị khóa.");

            var leaveRequest = await _context.LeaveRequests
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == request.LeaveRequestId, cancellationToken);

            if (leaveRequest == null)
                return Result.Failure("Không tìm thấy đơn xin nghỉ.");

            try
            {
                leaveRequest.AdminApprove(
                    request.AdminUserId,
                    request.AdminNote,
                    DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            // Thông báo cho học viên
            var studentNotification = new Notification(
                leaveRequest.Student.UserId,
                "Đơn xin nghỉ đã được Admin xác nhận",
                $"Đơn xin nghỉ lớp \"{leaveRequest.Course.Name}\" ngày {leaveRequest.LeaveDate:dd/MM/yyyy} đã được Admin xác nhận.",
                NotificationType.LeaveRequest,
                leaveRequest.Id,
                "LeaveRequest"
            );

            _context.Notifications.Add(studentNotification);

            // Thông báo lại cho giảng viên đã duyệt đơn
            if (leaveRequest.TeacherId.HasValue)
            {
                var teacherNotification = new Notification(
                    leaveRequest.TeacherId.Value,
                    "Admin đã xác nhận đơn xin nghỉ",
                    $"Admin đã xác nhận đơn xin nghỉ của học viên {leaveRequest.Student.User.FullName} trong lớp \"{leaveRequest.Course.Name}\".",
                    NotificationType.LeaveRequest,
                    leaveRequest.Id,
                    "LeaveRequest"
                );

                _context.Notifications.Add(teacherNotification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}