using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Grades.Queries.GetStudentGrades
{
    // Handler xử lý học viên xem điểm và nhận xét
    public class GetStudentGradesQueryHandler : IRequestHandler<GetStudentGradesQuery, Result<List<StudentGradeDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentGradesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentGradeDto>>> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
        {
            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<StudentGradeDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            // Lấy các đăng ký lớp đang học của học viên
            var enrollmentIds = await _context.Enrollments
                .Where(x =>
                    x.StudentId == student.Id &&
                    x.Status == EnrollmentStatus.Active)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            if (!enrollmentIds.Any())
                return Result<List<StudentGradeDto>>.Success(new List<StudentGradeDto>());

            // Lấy các bài nộp đã được chấm điểm
            var grades = await _context.Submissions
                .Include(x => x.Assignment)
                    .ThenInclude(x => x.Course)
                .Include(x => x.Assignment)
                    .ThenInclude(x => x.Teacher)
                .Include(x => x.Grade)
                .Where(x =>
                    enrollmentIds.Contains(x.EnrollmentId) &&
                    x.Grade != null)
                .OrderByDescending(x => x.Grade!.GradedAt)
                .Select(x => new StudentGradeDto
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentTitle = x.Assignment.Title,
                    CourseId = x.Assignment.CourseId,
                    CourseName = x.Assignment.Course.Name,
                    TeacherName = x.Assignment.Teacher.FullName,
                    SubmissionId = x.Id,
                    SubmissionContent = x.Content,
                    SubmittedFileName = x.SubmittedFileName,
                    SubmittedFileUrl = x.SubmittedFileUrl,
                    SubmittedAt = x.SubmittedAt,
                    SubmissionStatus = x.Status,
                    MaxScore = x.Assignment.MaxScore,
                    Score = x.Grade!.Score,
                    Feedback = x.Grade.Feedback,
                    GradedAt = x.Grade.GradedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<StudentGradeDto>>.Success(grades);
        }
    }
}