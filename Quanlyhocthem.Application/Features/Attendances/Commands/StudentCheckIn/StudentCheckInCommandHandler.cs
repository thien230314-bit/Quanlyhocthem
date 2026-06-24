using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.StudentCheckIn
{
    // Handler xử lý học viên điểm danh bằng mã
    public class StudentCheckInCommandHandler : IRequestHandler<StudentCheckInCommand, Result>
    {
        private readonly IAppDbContext _context;

        public StudentCheckInCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(StudentCheckInCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.AttendanceCode))
                return Result.Failure("Mã điểm danh không được để trống.");

            var code = request.AttendanceCode.Trim().ToUpper();
            var now = DateTime.UtcNow;

            // Tìm hồ sơ học viên theo UserId
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result.Failure("Không tìm thấy hồ sơ học viên.");

            // Tìm ca điểm danh theo mã
            var session = await _context.AttendanceSessions
                .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

            if (session == null)
                return Result.Failure("Mã điểm danh không tồn tại.");

            if (!session.CanCheckIn(now))
                return Result.Failure("Ca điểm danh đã hết hạn hoặc đã đóng.");

            // Kiểm tra học viên có đăng ký lớp này không
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == session.CourseId &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (enrollment == null)
                return Result.Failure("Bạn không thuộc lớp đang mở điểm danh.");

            // Tìm bản ghi điểm danh đã được tạo sẵn khi teacher mở ca
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(x =>
                    x.AttendanceSessionId == session.Id &&
                    x.EnrollmentId == enrollment.Id,
                    cancellationToken);

            if (attendance == null)
                return Result.Failure("Không tìm thấy bản ghi điểm danh của bạn trong ca này.");

            if (attendance.Status == AttendanceStatus.Present)
                return Result.Failure("Bạn đã điểm danh ca này rồi.");

            // Cập nhật điểm danh thành Present
            attendance.StudentCheckIn("Code", now);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}