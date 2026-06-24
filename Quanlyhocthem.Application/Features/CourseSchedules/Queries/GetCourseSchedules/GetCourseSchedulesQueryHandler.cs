using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseSchedules
{
    // Handler lấy danh sách lịch học cho Admin
    public class GetCourseSchedulesQueryHandler : IRequestHandler<GetCourseSchedulesQuery, Result<List<CourseScheduleDto>>>
    {
        private readonly IAppDbContext _context;

        public GetCourseSchedulesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<CourseScheduleDto>>> Handle(GetCourseSchedulesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<CourseSchedule> query = _context.CourseSchedules;

            if (request.CourseId.HasValue)
            {
                query = query.Where(x => x.CourseId == request.CourseId.Value);
            }

            if (request.TeacherId.HasValue)
            {
                query = query.Where(x => x.TeacherId == request.TeacherId.Value);
            }

            if (request.ClassroomId.HasValue)
            {
                query = query.Where(x => x.ClassroomId == request.ClassroomId.Value);
            }

            if (request.DayOfWeek.HasValue)
            {
                query = query.Where(x => x.DayOfWeek == request.DayOfWeek.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var schedules = await query
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .Select(x => new CourseScheduleDto
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
                    IsActive = x.IsActive,
                    Note = x.Note
                })
                .ToListAsync(cancellationToken);

            return Result<List<CourseScheduleDto>>.Success(schedules);
        }
    }
}