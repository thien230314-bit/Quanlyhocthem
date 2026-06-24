using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourses
{
    // Handler lấy danh sách lớp học được gán cho giảng viên
    public class GetTeacherCoursesQueryHandler : IRequestHandler<GetTeacherCoursesQuery, Result<List<TeacherCourseDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherCoursesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherCourseDto>>> Handle(GetTeacherCoursesQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra tài khoản hiện tại có phải giảng viên không
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<TeacherCourseDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<TeacherCourseDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            // Lấy các lớp được Admin gán cho giảng viên này
            var courses = await _context.Courses
                .Include(x => x.Subject)
                .Include(x => x.Batch)
                .Include(x => x.Classroom)
                .Include(x => x.Enrollments)
                .Where(x => x.TeacherId == request.TeacherUserId)
                .OrderBy(x => x.Name)
                .Select(x => new TeacherCourseDto
                {
                    CourseId = x.Id,
                    CourseName = x.Name,
                    Description = x.Description,
                    SubjectName = x.Subject.Name,
                    BatchName = x.Batch.Name,
                    ClassroomName = x.Classroom != null ? x.Classroom.Name : null,
                    TuitionFee = x.TuitionFee,
                    MaxStudents = x.MaxStudents,
                    CurrentStudents = x.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherCourseDto>>.Success(courses);
        }
    }
}