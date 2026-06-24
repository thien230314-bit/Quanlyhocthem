using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Users.Commands.DeleteUser
{
    // Handler xử lý xóa tài khoản
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return Result.Failure("Id tài khoản không hợp lệ.");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null)
                return Result.Failure("Không tìm thấy tài khoản.");

            // Không cho xóa Admin cuối cùng
            if (user.Role == Role.Admin)
            {
                var adminCount = await _context.Users
                    .CountAsync(x => x.Role == Role.Admin, cancellationToken);

                if (adminCount <= 1)
                    return Result.Failure("Không thể xóa Admin cuối cùng của hệ thống.");
            }

            // Nếu là giảng viên đã phụ trách lớp thì không cho xóa
            var hasCoursesAsTeacher = await _context.Courses
                .AnyAsync(x => x.TeacherId == request.UserId, cancellationToken);

            if (hasCoursesAsTeacher)
                return Result.Failure("Không thể xóa tài khoản vì giảng viên đã được gán vào lớp học.");

            // Nếu là giảng viên đã tạo bài tập thì không cho xóa
            var hasAssignmentsAsTeacher = await _context.Assignments
                .AnyAsync(x => x.TeacherId == request.UserId, cancellationToken);

            if (hasAssignmentsAsTeacher)
                return Result.Failure("Không thể xóa tài khoản vì giảng viên đã tạo bài tập.");

            // Nếu là giảng viên đã chấm điểm thì không cho xóa
            var hasGradesAsTeacher = await _context.Grades
                .AnyAsync(x => x.TeacherId == request.UserId, cancellationToken);

            if (hasGradesAsTeacher)
                return Result.Failure("Không thể xóa tài khoản vì giảng viên đã chấm điểm.");

            // Nếu là giảng viên đã mở ca điểm danh thì không cho xóa
            var hasAttendanceSessionsAsTeacher = await _context.AttendanceSessions
                .AnyAsync(x => x.TeacherId == request.UserId, cancellationToken);

            if (hasAttendanceSessionsAsTeacher)
                return Result.Failure("Không thể xóa tài khoản vì giảng viên đã mở ca điểm danh.");

            // Nếu là học viên thì kiểm tra hồ sơ học viên
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (student != null)
            {
                // Nếu học viên đã đăng ký lớp thì không cho xóa
                var hasEnrollments = await _context.Enrollments
                    .AnyAsync(x => x.StudentId == student.Id, cancellationToken);

                if (hasEnrollments)
                    return Result.Failure("Không thể xóa tài khoản vì học viên đã có dữ liệu đăng ký lớp.");

                // Nếu học viên đã có hóa đơn học phí thì không cho xóa
                var hasTuitionInvoices = await _context.TuitionInvoices
                    .AnyAsync(x => x.StudentId == student.Id, cancellationToken);

                if (hasTuitionInvoices)
                    return Result.Failure("Không thể xóa tài khoản vì học viên đã có hóa đơn học phí.");

                _context.Students.Remove(student);
            }

            // Xóa thông báo của tài khoản trước để tránh lỗi khóa ngoại
            var notifications = await _context.Notifications
                .Where(x => x.ReceiverUserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}