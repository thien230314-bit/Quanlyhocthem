using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.UpdateAssignment
{
    // Handler xử lý giảng viên cập nhật bài tập
    public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateAssignmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra tiêu đề
            if (string.IsNullOrWhiteSpace(request.Title))
                return Result.Failure("Tiêu đề bài tập không được để trống.");

            // Kiểm tra điểm tối đa
            if (request.MaxScore <= 0)
                return Result.Failure("Điểm tối đa phải lớn hơn 0.");

            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result.Failure("Tài khoản giảng viên đang bị khóa.");

            // Tìm bài tập kèm lớp học
            var assignment = await _context.Assignments
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.AssignmentId, cancellationToken);

            if (assignment == null)
                return Result.Failure("Không tìm thấy bài tập.");

            if (!assignment.IsActive)
                return Result.Failure("Bài tập đã bị khóa, không thể cập nhật.");

            // Chỉ giảng viên tạo bài hoặc giảng viên phụ trách lớp mới được cập nhật
            if (assignment.TeacherId != request.TeacherUserId && assignment.Course.TeacherId != request.TeacherUserId)
                return Result.Failure("Bạn không có quyền cập nhật bài tập này.");

            // Nếu đã có bài nộp được chấm thì không cho giảm MaxScore thấp hơn điểm đã chấm cao nhất
            var highestScore = await _context.Grades
                .Where(x => x.Submission.AssignmentId == assignment.Id)
                .Select(x => (decimal?)x.Score)
                .MaxAsync(cancellationToken);

            if (highestScore.HasValue && request.MaxScore < highestScore.Value)
                return Result.Failure($"Điểm tối đa mới không được nhỏ hơn điểm đã chấm cao nhất là {highestScore.Value}.");

            assignment.Update(
                request.Title,
                request.Description,
                request.DueDate,
                request.MaxScore,
                request.AttachmentFileName,
                request.AttachmentUrl
            );

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}