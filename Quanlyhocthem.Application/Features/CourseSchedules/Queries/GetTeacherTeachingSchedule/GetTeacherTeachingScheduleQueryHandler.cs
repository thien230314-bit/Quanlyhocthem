using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetTeacherTeachingSchedule
{
    // Handler lấy lịch dạy tuần của giảng viên
    public class GetTeacherTeachingScheduleQueryHandler : IRequestHandler<GetTeacherTeachingScheduleQuery, Result<List<TeacherTeachingScheduleDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherTeachingScheduleQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherTeachingScheduleDto>>> Handle(GetTeacherTeachingScheduleQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<TeacherTeachingScheduleDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<TeacherTeachingScheduleDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<List<TeacherTeachingScheduleDto>>.Failure("Tài khoản giảng viên đang bị khóa.");

            var weekDate = (request.WeekDate ?? DateTime.UtcNow).Date;
            var weekStart = GetStartOfWeek(weekDate);
            var weekEnd = weekStart.AddDays(6);

            IQueryable<CourseSchedule> query = _context.CourseSchedules;

            query = query.Where(x =>
                x.IsActive &&
                x.TeacherId == request.TeacherUserId &&
                x.EffectiveFrom <= weekEnd &&
                (x.EffectiveTo == null || x.EffectiveTo >= weekStart));

            if (request.CourseId.HasValue)
            {
                query = query.Where(x => x.CourseId == request.CourseId.Value);
            }

            var schedules = await query
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .Select(x => new TeacherTeachingScheduleDto
                {
                    CourseScheduleId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    SubjectName = x.Course.Subject.Name,
                    ClassroomId = x.ClassroomId,
                    ClassroomName = x.Classroom.Name,
                    ClassroomCode = x.Classroom.Code,
                    DayOfWeek = x.DayOfWeek,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo,
                    StudentCount = x.Course.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                    Note = x.Note
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherTeachingScheduleDto>>.Success(schedules);
        }

        // Lấy thứ 2 đầu tuần
        private static DateTime GetStartOfWeek(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }
    }
}