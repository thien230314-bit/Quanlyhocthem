using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseScheduleById
{
    // Handler lấy chi tiết lịch học
    public class GetCourseScheduleByIdQueryHandler : IRequestHandler<GetCourseScheduleByIdQuery, Result<CourseScheduleDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetCourseScheduleByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CourseScheduleDetailDto>> Handle(GetCourseScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.CourseScheduleId == Guid.Empty)
                return Result<CourseScheduleDetailDto>.Failure("Id lịch học không hợp lệ.");

            var schedule = await _context.CourseSchedules
                .Where(x => x.Id == request.CourseScheduleId)
                .Select(x => new CourseScheduleDetailDto
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
                    Note = x.Note,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (schedule == null)
                return Result<CourseScheduleDetailDto>.Failure("Không tìm thấy lịch học.");

            return Result<CourseScheduleDetailDto>.Success(schedule);
        }
    }
}