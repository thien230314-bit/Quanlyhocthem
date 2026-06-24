using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetTeacherCourseAssignments
{
    // Handler xử lý giảng viên xem bài tập đã giao theo lớp
    public class GetTeacherCourseAssignmentsQueryHandler : IRequestHandler<GetTeacherCourseAssignmentsQuery, Result<List<TeacherCourseAssignmentDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherCourseAssignmentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherCourseAssignmentDto>>> Handle(GetTeacherCourseAssignmentsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<TeacherCourseAssignmentDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<TeacherCourseAssignmentDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<List<TeacherCourseAssignmentDto>>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Kiểm tra lớp học
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<List<TeacherCourseAssignmentDto>>.Failure("Không tìm thấy lớp học.");

            // Chỉ giảng viên phụ trách lớp mới được xem bài tập của lớp đó
            if (course.TeacherId != request.TeacherUserId)
                return Result<List<TeacherCourseAssignmentDto>>.Failure("Bạn không có quyền xem bài tập của lớp này.");

            // Lấy danh sách bài tập của lớp
            var assignments = await _context.Assignments
                .Where(x => x.CourseId == request.CourseId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TeacherCourseAssignmentDto
                {
                    AssignmentId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Title = x.Title,
                    Description = x.Description,
                    DueDate = x.DueDate,
                    MaxScore = x.MaxScore,
                    AttachmentFileName = x.AttachmentFileName,
                    AttachmentUrl = x.AttachmentUrl,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    TotalSubmissions = x.Submissions.Count,
                    GradedSubmissions = x.Submissions.Count(s => s.Grade != null),
                    UngradedSubmissions = x.Submissions.Count(s => s.Grade == null)
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherCourseAssignmentDto>>.Success(assignments);
        }
    }
}