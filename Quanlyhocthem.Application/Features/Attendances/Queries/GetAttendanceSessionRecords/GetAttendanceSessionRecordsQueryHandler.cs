using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Attendances.Queries.GetAttendanceSessionRecords
{
    // Handler để giảng viên xem kết quả điểm danh
    public class GetAttendanceSessionRecordsQueryHandler : IRequestHandler<GetAttendanceSessionRecordsQuery, Result<AttendanceSessionRecordsDto>>
    {
        private readonly IAppDbContext _context;

        public GetAttendanceSessionRecordsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AttendanceSessionRecordsDto>> Handle(GetAttendanceSessionRecordsQuery request, CancellationToken cancellationToken)
        {
            // Tìm ca điểm danh
            var session = await _context.AttendanceSessions
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.AttendanceSessionId, cancellationToken);

            if (session == null)
                return Result<AttendanceSessionRecordsDto>.Failure("Không tìm thấy ca điểm danh.");

            // Chỉ giảng viên mở ca hoặc giảng viên phụ trách lớp mới được xem
            if (session.TeacherId != request.TeacherUserId && session.Course.TeacherId != request.TeacherUserId)
                return Result<AttendanceSessionRecordsDto>.Failure("Bạn không có quyền xem ca điểm danh này.");

            // Lấy danh sách bản ghi điểm danh
            var records = await _context.Attendances
                .Include(x => x.Enrollment)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.User)
                .Where(x => x.AttendanceSessionId == request.AttendanceSessionId)
                .OrderBy(x => x.Enrollment.Student.User.FullName)
                .Select(x => new AttendanceRecordDto
                {
                    AttendanceId = x.Id,
                    AttendanceSessionId = x.AttendanceSessionId,
                    EnrollmentId = x.EnrollmentId,
                    StudentCode = x.Enrollment.Student.StudentCode,
                    StudentName = x.Enrollment.Student.User.FullName,
                    Email = x.Enrollment.Student.User.Email,
                    Status = x.Status,
                    Method = x.Method,
                    CheckedInAt = x.CheckedInAt,
                    Note = x.Note
                })
                .ToListAsync(cancellationToken);

            var result = new AttendanceSessionRecordsDto
            {
                AttendanceSessionId = session.Id,
                CourseName = session.Course.Name,
                AttendanceCode = session.Code,
                OpenedAt = session.OpenedAt,
                ExpiredAt = session.ExpiredAt,
                IsOpen = session.IsOpen,
                TotalStudents = records.Count,
                PresentCount = records.Count(x => x.Status == AttendanceStatus.Present),
                AbsentCount = records.Count(x => x.Status == AttendanceStatus.Absent),
                Records = records
            };

            return Result<AttendanceSessionRecordsDto>.Success(result);
        }
    }
}