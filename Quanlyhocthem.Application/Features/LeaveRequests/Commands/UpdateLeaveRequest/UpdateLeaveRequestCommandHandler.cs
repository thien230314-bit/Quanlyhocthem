using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest
{
    // Handler xử lý học viên sửa đơn xin nghỉ
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.LeaveRequestId == Guid.Empty)
                return Result.Failure("Id đơn xin nghỉ không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Reason))
                return Result.Failure("Lý do xin nghỉ không được để trống.");

            if (request.LeaveDate.Date < DateTime.UtcNow.Date)
                return Result.Failure("Không thể xin nghỉ cho ngày trong quá khứ.");

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
                return Result.Failure("Chỉ được sửa đơn khi đang chờ giảng viên duyệt.");

            var duplicateExists = await _context.LeaveRequests
                .AnyAsync(x =>
                    x.Id != request.LeaveRequestId &&
                    x.StudentId == student.Id &&
                    x.CourseId == leaveRequest.CourseId &&
                    x.LeaveDate == request.LeaveDate.Date &&
                    x.Status != LeaveRequestStatus.Cancelled &&
                    x.Status != LeaveRequestStatus.RejectedByTeacher &&
                    x.Status != LeaveRequestStatus.RejectedByAdmin,
                    cancellationToken);

            if (duplicateExists)
                return Result.Failure("Bạn đã có đơn xin nghỉ khác cho lớp này trong ngày này.");

            try
            {
                leaveRequest.Update(request.LeaveDate, request.Reason);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}