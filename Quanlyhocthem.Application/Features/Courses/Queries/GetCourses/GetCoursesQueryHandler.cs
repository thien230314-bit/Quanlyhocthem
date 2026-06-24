using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourses
{
    // Handler lấy danh sách lớp học
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, Result<List<CourseDto>>>
    {
        private readonly IAppDbContext _context;

        public GetCoursesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<CourseDto>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            // Lấy danh sách lớp học kèm môn, khóa, phòng, giảng viên và sĩ số
            var courses = await _context.Courses
                .Include(x => x.Subject)
                .Include(x => x.Batch)
                .Include(x => x.Classroom)
                .Include(x => x.Teacher)
                .Include(x => x.Enrollments)
                .OrderBy(x => x.Name)
                .Select(x => new CourseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    TuitionFee = x.TuitionFee,
                    MaxStudents = x.MaxStudents,
                    CurrentStudents = x.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                    SubjectName = x.Subject.Name,
                    BatchName = x.Batch.Name,
                    ClassroomName = x.Classroom != null ? x.Classroom.Name : null,
                    TeacherName = x.Teacher != null ? x.Teacher.FullName : null,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<CourseDto>>.Success(courses);
        }
    }
}