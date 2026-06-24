using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetMyCourses
{
    // Handler lấy danh sách khóa học đã đăng ký của học viên
    public class GetMyCoursesQueryHandler : IRequestHandler<GetMyCoursesQuery, Result<List<MyCourseDto>>>
    {
        private readonly IAppDbContext _context;

        public GetMyCoursesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<MyCourseDto>>> Handle(GetMyCoursesQuery request, CancellationToken cancellationToken)
        {
            // Tìm hồ sơ Student theo UserId
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<MyCourseDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            // Lấy danh sách khóa học đã đăng ký
            var courses = await _context.Enrollments
                .Include(x => x.Course)
                    .ThenInclude(c => c.Teacher)
                .Where(x => x.StudentId == student.Id)
                .Select(x => new MyCourseDto
                {
                    EnrollmentId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    TeacherName = x.Course.Teacher != null ? x.Course.Teacher.FullName : null,
                    EnrollmentDate = x.EnrollmentDate,
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            return Result<List<MyCourseDto>>.Success(courses);
        }
    }
}