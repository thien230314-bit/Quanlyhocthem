using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Submissions.Commands.SubmitAssignment
{
    // Handler xử lý học viên nộp bài
    public class SubmitAssignmentCommandHandler : IRequestHandler<SubmitAssignmentCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public SubmitAssignmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(SubmitAssignmentCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra học viên có nhập nội dung hoặc file không
            var hasContent = !string.IsNullOrWhiteSpace(request.Content);
            var hasFile = !string.IsNullOrWhiteSpace(request.SubmittedFileUrl);

            if (!hasContent && !hasFile)
                return Result<Guid>.Failure("Bài nộp phải có nội dung hoặc file đính kèm.");

            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<Guid>.Failure("Không tìm thấy hồ sơ học viên.");

            // Tìm bài tập
            var assignment = await _context.Assignments
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.AssignmentId, cancellationToken);

            if (assignment == null)
                return Result<Guid>.Failure("Không tìm thấy bài tập.");

            if (!assignment.IsActive)
                return Result<Guid>.Failure("Bài tập này đã ngưng hoạt động.");

            // Kiểm tra học viên có đăng ký lớp chứa bài tập không
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == assignment.CourseId &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (enrollment == null)
                return Result<Guid>.Failure("Bạn không thuộc lớp được giao bài tập này.");

            var now = DateTime.UtcNow;
            var isLate = now > assignment.DueDate;

            // Kiểm tra học viên đã từng nộp bài này chưa
            var existingSubmission = await _context.Submissions
                .FirstOrDefaultAsync(x =>
                    x.AssignmentId == assignment.Id &&
                    x.EnrollmentId == enrollment.Id,
                    cancellationToken);

            // Nếu đã chấm điểm rồi thì không cho nộp lại
            if (existingSubmission != null && existingSubmission.Status == SubmissionStatus.Graded)
                return Result<Guid>.Failure("Bài nộp đã được chấm điểm, không thể nộp lại.");

            // Nếu đã nộp rồi nhưng chưa chấm thì cho cập nhật bài nộp
            if (existingSubmission != null)
            {
                existingSubmission.UpdateSubmission(
                    request.Content,
                    request.SubmittedFileName,
                    request.SubmittedFileUrl,
                    now,
                    isLate
                );

                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(existingSubmission.Id);
            }

            // Tạo bài nộp mới
            var submission = new Submission(
                assignment.Id,
                enrollment.Id,
                request.Content,
                request.SubmittedFileName,
                request.SubmittedFileUrl,
                now,
                isLate
            );

            _context.Submissions.Add(submission);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(submission.Id);
        }
    }
}