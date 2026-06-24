using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Submissions.Queries.GetAssignmentSubmissions
{
    // Handler xử lý giảng viên xem danh sách bài nộp
    public class GetAssignmentSubmissionsQueryHandler : IRequestHandler<GetAssignmentSubmissionsQuery, Result<List<AssignmentSubmissionDto>>>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentSubmissionsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AssignmentSubmissionDto>>> Handle(GetAssignmentSubmissionsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<AssignmentSubmissionDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<AssignmentSubmissionDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<List<AssignmentSubmissionDto>>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Tìm bài tập
            var assignment = await _context.Assignments
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.AssignmentId, cancellationToken);

            if (assignment == null)
                return Result<List<AssignmentSubmissionDto>>.Failure("Không tìm thấy bài tập.");

            // Chỉ giảng viên tạo bài hoặc giảng viên phụ trách lớp mới được xem bài nộp
            if (assignment.TeacherId != request.TeacherUserId && assignment.Course.TeacherId != request.TeacherUserId)
                return Result<List<AssignmentSubmissionDto>>.Failure("Bạn không có quyền xem bài nộp của bài tập này.");

            // Lấy danh sách bài nộp của bài tập
            var submissions = await _context.Submissions
                .Include(x => x.Assignment)
                .Include(x => x.Enrollment)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.User)
                .Include(x => x.Grade)
                .Where(x => x.AssignmentId == request.AssignmentId)
                .OrderBy(x => x.Enrollment.Student.User.FullName)
                .Select(x => new AssignmentSubmissionDto
                {
                    SubmissionId = x.Id,
                    AssignmentId = x.AssignmentId,
                    AssignmentTitle = x.Assignment.Title,
                    StudentId = x.Enrollment.StudentId,
                    StudentCode = x.Enrollment.Student.StudentCode,
                    StudentName = x.Enrollment.Student.User.FullName,
                    StudentEmail = x.Enrollment.Student.User.Email,
                    Content = x.Content,
                    SubmittedFileName = x.SubmittedFileName,
                    SubmittedFileUrl = x.SubmittedFileUrl,
                    SubmittedAt = x.SubmittedAt,
                    Status = x.Status,
                    IsGraded = x.Grade != null,
                    Score = x.Grade != null ? x.Grade.Score : null,
                    Feedback = x.Grade != null ? x.Grade.Feedback : null,
                    GradedAt = x.Grade != null ? x.Grade.GradedAt : null
                })
                .ToListAsync(cancellationToken);

            return Result<List<AssignmentSubmissionDto>>.Success(submissions);
        }
    }
}