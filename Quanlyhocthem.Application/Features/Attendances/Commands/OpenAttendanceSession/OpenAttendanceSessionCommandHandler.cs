using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;
using System.Security.Cryptography;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.OpenAttendanceSession
{
    // Handler xử lý giảng viên mở ca điểm danh
    public class OpenAttendanceSessionCommandHandler : IRequestHandler<OpenAttendanceSessionCommand, Result<OpenAttendanceSessionResultDto>>
    {
        private readonly IAppDbContext _context;

        public OpenAttendanceSessionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<OpenAttendanceSessionResultDto>> Handle(OpenAttendanceSessionCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra thời lượng điểm danh
            if (request.DurationMinutes < 1)
                return Result<OpenAttendanceSessionResultDto>.Failure("Thời lượng điểm danh phải lớn hơn 0 phút.");

            if (request.DurationMinutes > 120)
                return Result<OpenAttendanceSessionResultDto>.Failure("Thời lượng điểm danh không được vượt quá 120 phút.");

            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<OpenAttendanceSessionResultDto>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<OpenAttendanceSessionResultDto>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<OpenAttendanceSessionResultDto>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Kiểm tra lớp học có thuộc giảng viên này không
            var course = await _context.Courses
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<OpenAttendanceSessionResultDto>.Failure("Không tìm thấy lớp học.");

            if (course.TeacherId != request.TeacherUserId)
                return Result<OpenAttendanceSessionResultDto>.Failure("Bạn không có quyền mở điểm danh cho lớp này.");

            if (!course.IsActive)
                return Result<OpenAttendanceSessionResultDto>.Failure("Không thể mở điểm danh cho lớp đã ngưng hoạt động.");

            // Không cho mở nhiều ca điểm danh đang hoạt động cùng lúc cho cùng một lớp
            var now = DateTime.UtcNow;

            var hasOpeningSession = await _context.AttendanceSessions
                .AnyAsync(x =>
                    x.CourseId == request.CourseId &&
                    x.IsOpen &&
                    x.ExpiredAt >= now,
                    cancellationToken);

            if (hasOpeningSession)
                return Result<OpenAttendanceSessionResultDto>.Failure("Lớp này đang có ca điểm danh còn hiệu lực.");

            // Lấy danh sách học viên đang đăng ký lớp
            var activeEnrollments = await _context.Enrollments
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active)
                .ToListAsync(cancellationToken);

            if (!activeEnrollments.Any())
                return Result<OpenAttendanceSessionResultDto>.Failure("Lớp học chưa có học viên đăng ký.");

            // Sinh mã điểm danh không trùng
            var attendanceCode = await GenerateUniqueAttendanceCode(cancellationToken);

            var openedAt = now;
            var expiredAt = now.AddMinutes(request.DurationMinutes);

            // Tạo ca điểm danh
            var session = new AttendanceSession(
                request.CourseId,
                request.TeacherUserId,
                attendanceCode,
                openedAt,
                expiredAt,
                request.Note
            );

            _context.AttendanceSessions.Add(session);

            // Tạo sẵn bản ghi Absent cho toàn bộ học viên
            // Student điểm danh thì bản ghi này sẽ được chuyển sang Present
            foreach (var enrollment in activeEnrollments)
            {
                var attendance = new Attendance(
                    session.Id,
                    enrollment.Id,
                    AttendanceStatus.Absent,
                    "System",
                    null,
                    "Chưa điểm danh"
                );

                _context.Attendances.Add(attendance);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var result = new OpenAttendanceSessionResultDto
            {
                AttendanceSessionId = session.Id,
                AttendanceCode = session.Code,
                OpenedAt = session.OpenedAt,
                ExpiredAt = session.ExpiredAt,
                TotalStudents = activeEnrollments.Count
            };

            return Result<OpenAttendanceSessionResultDto>.Success(result);
        }

        // Sinh mã điểm danh 6 số và kiểm tra không trùng database
        private async Task<string> GenerateUniqueAttendanceCode(CancellationToken cancellationToken)
        {
            while (true)
            {
                var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

                var exists = await _context.AttendanceSessions
                    .AnyAsync(x => x.Code == code, cancellationToken);

                if (!exists)
                    return code;
            }
        }
    }
}