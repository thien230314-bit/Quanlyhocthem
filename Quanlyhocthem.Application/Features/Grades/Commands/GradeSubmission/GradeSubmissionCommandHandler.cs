using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Grades.Commands.GradeSubmission
{
    // Handler xử lý giảng viên chấm điểm bài nộp
    public class GradeSubmissionCommandHandler : IRequestHandler<GradeSubmissionCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public GradeSubmissionCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(GradeSubmissionCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra điểm không được âm
            if (request.Score < 0)
                return Result<Guid>.Failure("Điểm không được nhỏ hơn 0.");

            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<Guid>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<Guid>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<Guid>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Tìm bài nộp kèm bài tập, lớp học, học viên và điểm
            var submission = await _context.Submissions
                .Include(x => x.Assignment)
                    .ThenInclude(x => x.Course)
                .Include(x => x.Enrollment)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.User)
                .Include(x => x.Grade)
                .FirstOrDefaultAsync(x => x.Id == request.SubmissionId, cancellationToken);

            if (submission == null)
                return Result<Guid>.Failure("Không tìm thấy bài nộp.");

            var assignment = submission.Assignment;

            // Chỉ giảng viên tạo bài tập hoặc giảng viên phụ trách lớp mới được chấm
            if (assignment.TeacherId != request.TeacherUserId && assignment.Course.TeacherId != request.TeacherUserId)
                return Result<Guid>.Failure("Bạn không có quyền chấm bài nộp này.");

            // Không cho chấm vượt điểm tối đa của bài tập
            if (request.Score > assignment.MaxScore)
                return Result<Guid>.Failure($"Điểm không được vượt quá điểm tối đa của bài tập là {assignment.MaxScore}.");

            var now = DateTime.UtcNow;
            Guid gradeId;

            // Nếu bài nộp đã có điểm thì cập nhật điểm và nhận xét
            if (submission.Grade != null)
            {
                submission.Grade.Update(request.Score, request.Feedback, now);
                submission.MarkAsGraded();

                gradeId = submission.Grade.Id;
            }
            else
            {
                // Nếu chưa có điểm thì tạo điểm mới
                var grade = new Grade(
                    submission.Id,
                    request.TeacherUserId,
                    request.Score,
                    request.Feedback,
                    now
                );

                _context.Grades.Add(grade);

                // Đánh dấu bài nộp đã được chấm
                submission.MarkAsGraded();

                gradeId = grade.Id;
            }

            // Tạo thông báo cho học viên được chấm điểm
            var notification = new Notification(
                submission.Enrollment.Student.UserId,
                "Bài nộp đã được chấm điểm",
                $"Bài nộp \"{assignment.Title}\" của bạn đã được chấm {request.Score}/{assignment.MaxScore}.",
                NotificationType.SubmissionGraded,
                gradeId,
                "Grade"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(gradeId);
        }
    }
}