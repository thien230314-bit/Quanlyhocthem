using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourseById
{
    // Handler lấy chi tiết lớp học
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetCourseByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CourseDetailDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result<CourseDetailDto>.Failure("Id lớp học không hợp lệ.");

            var course = await _context.Courses
                .Where(x => x.Id == request.CourseId)
                .Select(x => new CourseDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    TuitionFee = x.TuitionFee,
                    MaxStudents = x.MaxStudents,
                    CurrentStudents = x.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject.Name,
                    BatchId = x.BatchId,
                    BatchName = x.Batch.Name,
                    ClassroomId = x.ClassroomId,
                    ClassroomName = x.Classroom != null ? x.Classroom.Name : null,
                    TeacherId = x.TeacherId,
                    TeacherName = x.Teacher != null ? x.Teacher.FullName : null,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    TotalAssignments = x.Assignments.Count,
                    TotalAttendanceSessions = x.AttendanceSessions.Count,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (course == null)
                return Result<CourseDetailDto>.Failure("Không tìm thấy lớp học.");

            return Result<CourseDetailDto>.Success(course);
        }
    }
}