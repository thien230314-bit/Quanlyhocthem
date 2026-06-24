using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.TeacherApproveLeaveRequest
{
    // Handler xử lý giảng viên duyệt đơn xin nghỉ
    public class TeacherApproveLeaveRequestCommandHandler : IRequestHandler<TeacherApproveLeaveRequestCommand, Result>
    {
        private readonly IAppDbContext _context;

        public TeacherApproveLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(TeacherApproveLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.LeaveRequestId == Guid.Empty)
                return Result.Failure("Id đơn xin nghỉ không hợp lệ.");

            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result.Failure("Tài khoản giảng viên đang bị khóa.");

            var leaveRequest = await _context.LeaveRequests
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.LeaveRequestId, cancellationToken);

            if (leaveRequest == null)
                return Result.Failure("Không tìm thấy đơn xin nghỉ.");

            if (leaveRequest.Course.TeacherId != request.TeacherUserId)
                return Result.Failure("Bạn không có quyền duyệt đơn xin nghỉ này.");

            try
            {
                leaveRequest.TeacherApprove(
                    request.TeacherUserId,
                    request.TeacherNote,
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
                "Đơn xin nghỉ đã được giảng viên duyệt",
                $"Đơn xin nghỉ lớp \"{leaveRequest.Course.Name}\" ngày {leaveRequest.LeaveDate:dd/MM/yyyy} đã được giảng viên duyệt và gửi Admin xác nhận.",
                NotificationType.LeaveRequest,
                leaveRequest.Id,
                "LeaveRequest"
            );

            _context.Notifications.Add(studentNotification);

            // Thông báo cho toàn bộ Admin đang hoạt động
            var adminUserIds = await _context.Users
                .Where(x => x.Role == Role.Admin && x.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            foreach (var adminUserId in adminUserIds)
            {
                var adminNotification = new Notification(
                    adminUserId,
                    "Có đơn xin nghỉ chờ Admin xác nhận",
                    $"Giảng viên đã duyệt đơn xin nghỉ của học viên {leaveRequest.Student.User.FullName} trong lớp \"{leaveRequest.Course.Name}\".",
                    NotificationType.LeaveRequest,
                    leaveRequest.Id,
                    "LeaveRequest"
                );

                _context.Notifications.Add(adminNotification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}