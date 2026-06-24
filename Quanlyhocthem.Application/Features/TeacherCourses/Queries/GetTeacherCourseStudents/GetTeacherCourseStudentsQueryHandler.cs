using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourseStudents
{
    // Handler lấy danh sách học sinh trong lớp của giảng viên
    public class GetTeacherCourseStudentsQueryHandler : IRequestHandler<GetTeacherCourseStudentsQuery, Result<List<TeacherCourseStudentDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherCourseStudentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherCourseStudentDto>>> Handle(GetTeacherCourseStudentsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra lớp học có tồn tại không
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<List<TeacherCourseStudentDto>>.Failure("Không tìm thấy lớp học.");

            // Chỉ giảng viên được gán lớp này mới được xem danh sách học sinh
            if (course.TeacherId != request.TeacherUserId)
                return Result<List<TeacherCourseStudentDto>>.Failure("Bạn không có quyền xem danh sách học sinh của lớp này.");

            // Lấy danh sách học sinh đã đăng ký lớp
            var students = await _context.Enrollments
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Student)
                    .ThenInclude(x => x.Batch)
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active)
                .OrderBy(x => x.Student.User.FullName)
                .Select(x => new TeacherCourseStudentDto
                {
                    EnrollmentId = x.Id,
                    StudentId = x.StudentId,
                    StudentUserId = x.Student.UserId,
                    StudentCode = x.Student.StudentCode,
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber,
                    BatchName = x.Student.Batch.Name,
                    EnrollmentStatus = x.Status,
                    RegisteredAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherCourseStudentDto>>.Success(students);
        }
    }
}