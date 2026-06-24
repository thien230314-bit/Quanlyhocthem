using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.DeleteAssignment
{
    // Handler xử lý giảng viên xóa bài tập
    public class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteAssignmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            if (request.AssignmentId == Guid.Empty)
                return Result.Failure("Id bài tập không hợp lệ.");

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

            // Chỉ giảng viên tạo bài hoặc giảng viên phụ trách lớp mới được xóa
            if (assignment.TeacherId != request.TeacherUserId && assignment.Course.TeacherId != request.TeacherUserId)
                return Result.Failure("Bạn không có quyền xóa bài tập này.");

            // Nếu đã có học viên nộp bài thì không cho xóa
            var hasSubmissions = await _context.Submissions
                .AnyAsync(x => x.AssignmentId == request.AssignmentId, cancellationToken);

            if (hasSubmissions)
                return Result.Failure("Không thể xóa bài tập vì đã có học viên nộp bài.");

            // Xóa thông báo liên quan đến bài tập trước để tránh lỗi khóa ngoại logic
            var notifications = await _context.Notifications
                .Where(x =>
                    x.RelatedEntityId == request.AssignmentId &&
                    x.RelatedEntityType == "Assignment")
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
            }

            _context.Assignments.Remove(assignment);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}