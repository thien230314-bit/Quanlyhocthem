using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetAssignmentById
{
    // Handler lấy chi tiết bài tập
    public class GetAssignmentByIdQueryHandler : IRequestHandler<GetAssignmentByIdQuery, Result<AssignmentDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AssignmentDetailDto>> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.AssignmentId == Guid.Empty)
                return Result<AssignmentDetailDto>.Failure("Id bài tập không hợp lệ.");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.CurrentUserId, cancellationToken);

            if (user == null)
                return Result<AssignmentDetailDto>.Failure("Không tìm thấy tài khoản người dùng.");

            var assignment = await _context.Assignments
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .Include(x => x.Submissions)
                    .ThenInclude(x => x.Grade)
                .FirstOrDefaultAsync(x => x.Id == request.AssignmentId, cancellationToken);

            if (assignment == null)
                return Result<AssignmentDetailDto>.Failure("Không tìm thấy bài tập.");

            // Admin được xem tất cả
            if (user.Role == Role.Admin)
                return Result<AssignmentDetailDto>.Success(MapToDto(assignment));

            // Teacher chỉ được xem bài mình tạo hoặc bài của lớp mình phụ trách
            if (user.Role == Role.Teacher)
            {
                var isOwner = assignment.TeacherId == request.CurrentUserId;
                var isCourseTeacher = assignment.Course.TeacherId == request.CurrentUserId;

                if (!isOwner && !isCourseTeacher)
                    return Result<AssignmentDetailDto>.Failure("Bạn không có quyền xem bài tập này.");

                return Result<AssignmentDetailDto>.Success(MapToDto(assignment));
            }

            // Student chỉ được xem bài tập của lớp đã đăng ký
            if (user.Role == Role.Student)
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(x => x.UserId == request.CurrentUserId, cancellationToken);

                if (student == null)
                    return Result<AssignmentDetailDto>.Failure("Không tìm thấy hồ sơ học viên.");

                var hasEnrollment = await _context.Enrollments
                    .AnyAsync(x =>
                        x.StudentId == student.Id &&
                        x.CourseId == assignment.CourseId &&
                        x.Status == EnrollmentStatus.Active,
                        cancellationToken);

                if (!hasEnrollment)
                    return Result<AssignmentDetailDto>.Failure("Bạn không thuộc lớp được giao bài tập này.");

                return Result<AssignmentDetailDto>.Success(MapToDto(assignment));
            }

            return Result<AssignmentDetailDto>.Failure("Vai trò không hợp lệ.");
        }

        private static AssignmentDetailDto MapToDto(Domain.Entities.Assignment assignment)
        {
            return new AssignmentDetailDto
            {
                AssignmentId = assignment.Id,
                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,
                TeacherId = assignment.TeacherId,
                TeacherName = assignment.Teacher.FullName,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate,
                MaxScore = assignment.MaxScore,
                AttachmentFileName = assignment.AttachmentFileName,
                AttachmentUrl = assignment.AttachmentUrl,
                IsActive = assignment.IsActive,
                TotalSubmissions = assignment.Submissions.Count,
                GradedSubmissions = assignment.Submissions.Count(x => x.Grade != null),
                UngradedSubmissions = assignment.Submissions.Count(x => x.Grade == null),
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };
        }
    }
}