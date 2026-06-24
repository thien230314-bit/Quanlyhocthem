using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.CreateAssignment
{
    // Handler xử lý giảng viên tạo bài tập
    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateAssignmentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra tiêu đề bài tập
            if (string.IsNullOrWhiteSpace(request.Title))
                return Result<Guid>.Failure("Tiêu đề bài tập không được để trống.");

            // Kiểm tra hạn nộp
            if (request.DueDate <= DateTime.UtcNow)
                return Result<Guid>.Failure("Hạn nộp bài phải lớn hơn thời gian hiện tại.");

            // Kiểm tra điểm tối đa
            if (request.MaxScore <= 0)
                return Result<Guid>.Failure("Điểm tối đa phải lớn hơn 0.");

            // Kiểm tra tài khoản giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<Guid>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<Guid>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<Guid>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Kiểm tra lớp học
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<Guid>.Failure("Không tìm thấy lớp học.");

            // Chỉ giảng viên được phân công lớp mới được tạo bài tập
            if (course.TeacherId != request.TeacherUserId)
                return Result<Guid>.Failure("Bạn không có quyền tạo bài tập cho lớp này.");

            if (!course.IsActive)
                return Result<Guid>.Failure("Không thể tạo bài tập cho lớp đã ngưng hoạt động.");

            // Tạo bài tập
            var assignment = new Assignment(
                request.CourseId,
                request.TeacherUserId,
                request.Title,
                request.Description,
                request.DueDate,
                request.MaxScore,
                request.AttachmentFileName,
                request.AttachmentUrl
            );

            _context.Assignments.Add(assignment);

            // Lấy danh sách học viên đang học trong lớp để gửi thông báo
            var studentUserIds = await _context.Enrollments
                .Include(x => x.Student)
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active &&
                    x.Student.UserId != Guid.Empty)
                .Select(x => x.Student.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var studentUserId in studentUserIds)
            {
                var notification = new Notification(
                    studentUserId,
                    "Có bài tập mới",
                    $"Giảng viên đã giao bài tập mới \"{assignment.Title}\" cho lớp \"{course.Name}\".",
                    NotificationType.AssignmentCreated,
                    assignment.Id,
                    "Assignment"
                );

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(assignment.Id);
        }
    }
}