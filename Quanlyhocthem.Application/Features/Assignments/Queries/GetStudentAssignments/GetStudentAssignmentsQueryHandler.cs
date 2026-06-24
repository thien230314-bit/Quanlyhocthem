using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetStudentAssignments
{
    // Handler lấy danh sách bài tập của học viên
    public class GetStudentAssignmentsQueryHandler : IRequestHandler<GetStudentAssignmentsQuery, Result<List<StudentAssignmentDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentAssignmentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentAssignmentDto>>> Handle(GetStudentAssignmentsQuery request, CancellationToken cancellationToken)
        {
            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<StudentAssignmentDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            // Lấy các enrollment đang học của học viên
            var enrollments = await _context.Enrollments
                .Where(x =>
                    x.StudentId == student.Id &&
                    x.Status == EnrollmentStatus.Active)
                .ToListAsync(cancellationToken);

            if (!enrollments.Any())
                return Result<List<StudentAssignmentDto>>.Success(new List<StudentAssignmentDto>());

            var courseIds = enrollments.Select(x => x.CourseId).ToList();
            var enrollmentIds = enrollments.Select(x => x.Id).ToList();

            // Lấy bài tập thuộc các lớp học viên đã đăng ký
            var assignments = await _context.Assignments
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .Include(x => x.Submissions)
                .Where(x =>
                    courseIds.Contains(x.CourseId) &&
                    x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var now = DateTime.UtcNow;

            // Map dữ liệu trả ra cho học viên
            var result = assignments.Select(assignment =>
            {
                var submission = assignment.Submissions
                    .FirstOrDefault(x => enrollmentIds.Contains(x.EnrollmentId));

                return new StudentAssignmentDto
                {
                    AssignmentId = assignment.Id,
                    CourseId = assignment.CourseId,
                    CourseName = assignment.Course.Name,
                    TeacherName = assignment.Teacher.FullName,
                    Title = assignment.Title,
                    Description = assignment.Description,
                    DueDate = assignment.DueDate,
                    MaxScore = assignment.MaxScore,
                    AttachmentFileName = assignment.AttachmentFileName,
                    AttachmentUrl = assignment.AttachmentUrl,
                    IsSubmitted = submission != null,
                    IsOverdue = submission == null && assignment.DueDate < now,
                    SubmissionId = submission?.Id,
                    SubmittedAt = submission?.SubmittedAt,
                    SubmissionStatus = submission?.Status
                };
            }).ToList();

            return Result<List<StudentAssignmentDto>>.Success(result);
        }
    }
}