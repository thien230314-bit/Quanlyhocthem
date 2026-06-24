using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.UpdateAttendanceRecord
{
    // Handler để giảng viên sửa điểm danh thủ công
    public class UpdateAttendanceRecordCommandHandler : IRequestHandler<UpdateAttendanceRecordCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateAttendanceRecordCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateAttendanceRecordCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra trạng thái điểm danh hợp lệ
            if (!Enum.IsDefined(typeof(AttendanceStatus), request.Status))
                return Result.Failure("Trạng thái điểm danh không hợp lệ.");

            // Tìm bản ghi điểm danh
            var attendance = await _context.Attendances
                .Include(x => x.AttendanceSession)
                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.AttendanceId, cancellationToken);

            if (attendance == null)
                return Result.Failure("Không tìm thấy bản ghi điểm danh.");

            // Chỉ giảng viên mở ca hoặc giảng viên phụ trách lớp mới được sửa
            var session = attendance.AttendanceSession;

            if (session.TeacherId != request.TeacherUserId && session.Course.TeacherId != request.TeacherUserId)
                return Result.Failure("Bạn không có quyền sửa bản ghi điểm danh này.");

            // Sửa điểm danh
            attendance.TeacherUpdate(request.Status, request.Note);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}