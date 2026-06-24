using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetAvailableCourses
{
    // Handler lấy khóa học học viên có thể đăng ký
    public class GetAvailableCoursesQueryHandler : IRequestHandler<GetAvailableCoursesQuery, Result<List<AvailableCourseDto>>>
    {
        private readonly IAppDbContext _context;

        public GetAvailableCoursesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AvailableCourseDto>>> Handle(GetAvailableCoursesQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra user hiện tại có hồ sơ Student không
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<AvailableCourseDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            // Chỉ lấy các Course cùng Batch với học viên
            // Đồng thời loại bỏ Course mà học viên đã đăng ký active
            var courses = await _context.Courses
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.Batch)
                .Include(x => x.Classroom)
                .Include(x => x.Enrollments)
                .Where(course =>
                    course.IsActive &&
                    course.BatchId == student.BatchId &&
                    !course.Enrollments.Any(e =>
                        e.StudentId == student.Id &&
                        e.Status == EnrollmentStatus.Active))
                .Select(course => new AvailableCourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    TuitionFee = course.TuitionFee,
                    MaxStudents = course.MaxStudents,
                    CurrentStudents = course.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                    TeacherName = course.Teacher != null ? course.Teacher.FullName : null
                })
                .ToListAsync(cancellationToken);

            return Result<List<AvailableCourseDto>>.Success(courses);
        }
    }
}