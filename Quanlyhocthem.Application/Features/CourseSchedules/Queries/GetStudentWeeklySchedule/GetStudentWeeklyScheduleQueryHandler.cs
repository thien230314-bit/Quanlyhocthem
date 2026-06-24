using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetStudentWeeklySchedule
{
    // Handler lấy thời khóa biểu tuần của học viên
    public class GetStudentWeeklyScheduleQueryHandler : IRequestHandler<GetStudentWeeklyScheduleQuery, Result<List<StudentWeeklyScheduleDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentWeeklyScheduleQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentWeeklyScheduleDto>>> Handle(GetStudentWeeklyScheduleQuery request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<StudentWeeklyScheduleDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            var weekDate = (request.WeekDate ?? DateTime.UtcNow).Date;
            var weekStart = GetStartOfWeek(weekDate);
            var weekEnd = weekStart.AddDays(6);

            // Lấy danh sách lớp học viên đã đăng ký
            var courseIds = await _context.Enrollments
                .Where(x =>
                    x.StudentId == student.Id &&
                    x.Status == EnrollmentStatus.Active)
                .Select(x => x.CourseId)
                .ToListAsync(cancellationToken);

            if (!courseIds.Any())
                return Result<List<StudentWeeklyScheduleDto>>.Success(new List<StudentWeeklyScheduleDto>());

            IQueryable<CourseSchedule> query = _context.CourseSchedules;

            var schedules = await query
                .Where(x =>
                    x.IsActive &&
                    courseIds.Contains(x.CourseId) &&
                    x.EffectiveFrom <= weekEnd &&
                    (x.EffectiveTo == null || x.EffectiveTo >= weekStart))
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .Select(x => new StudentWeeklyScheduleDto
                {
                    CourseScheduleId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    SubjectName = x.Course.Subject.Name,
                    TeacherId = x.TeacherId,
                    TeacherName = x.Teacher.FullName,
                    ClassroomId = x.ClassroomId,
                    ClassroomName = x.Classroom.Name,
                    ClassroomCode = x.Classroom.Code,
                    DayOfWeek = x.DayOfWeek,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo,
                    Note = x.Note
                })
                .ToListAsync(cancellationToken);

            return Result<List<StudentWeeklyScheduleDto>>.Success(schedules);
        }

        // Lấy thứ 2 đầu tuần
        private static DateTime GetStartOfWeek(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }
    }
}