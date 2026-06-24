using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.TeacherRejectLeaveRequest
{
    // Handler xử lý giảng viên từ chối đơn xin nghỉ
    public class TeacherRejectLeaveRequestCommandHandler : IRequestHandler<TeacherRejectLeaveRequestCommand, Result>
    {
        private readonly IAppDbContext _context;

        public TeacherRejectLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(TeacherRejectLeaveRequestCommand request, CancellationToken cancellationToken)
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
                return Result.Failure("Bạn không có quyền từ chối đơn xin nghỉ này.");

            try
            {
                leaveRequest.TeacherReject(
                    request.TeacherUserId,
                    request.TeacherNote,
                    DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            var notification = new Notification(
                leaveRequest.Student.UserId,
                "Đơn xin nghỉ bị giảng viên từ chối",
                $"Đơn xin nghỉ lớp \"{leaveRequest.Course.Name}\" ngày {leaveRequest.LeaveDate:dd/MM/yyyy} đã bị giảng viên từ chối.",
                NotificationType.LeaveRequest,
                leaveRequest.Id,
                "LeaveRequest"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}