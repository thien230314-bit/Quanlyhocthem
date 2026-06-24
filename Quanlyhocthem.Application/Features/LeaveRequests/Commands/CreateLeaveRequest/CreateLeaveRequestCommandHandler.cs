using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    // Handler xử lý học viên gửi đơn xin nghỉ
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateLeaveRequestCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result<Guid>.Failure("Id lớp học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Reason))
                return Result<Guid>.Failure("Lý do xin nghỉ không được để trống.");

            if (request.LeaveDate.Date < DateTime.UtcNow.Date)
                return Result<Guid>.Failure("Không thể xin nghỉ cho ngày trong quá khứ.");

            // Tìm học viên theo UserId trong token
            var student = await _context.Students
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<Guid>.Failure("Không tìm thấy hồ sơ học viên.");

            if (!student.User.IsActive)
                return Result<Guid>.Failure("Tài khoản học viên đang bị khóa.");

            // Tìm lớp học
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<Guid>.Failure("Không tìm thấy lớp học.");

            if (!course.IsActive)
                return Result<Guid>.Failure("Lớp học đã ngưng hoạt động.");

            // Kiểm tra học viên có thuộc lớp không
            var hasEnrollment = await _context.Enrollments
                .AnyAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (!hasEnrollment)
                return Result<Guid>.Failure("Bạn chưa đăng ký lớp học này.");

            // Không cho gửi trùng đơn cùng lớp, cùng ngày nếu đơn đang còn xử lý
            var duplicateExists = await _context.LeaveRequests
                .AnyAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == request.CourseId &&
                    x.LeaveDate == request.LeaveDate.Date &&
                    x.Status != LeaveRequestStatus.Cancelled &&
                    x.Status != LeaveRequestStatus.RejectedByTeacher &&
                    x.Status != LeaveRequestStatus.RejectedByAdmin,
                    cancellationToken);

            if (duplicateExists)
                return Result<Guid>.Failure("Bạn đã gửi đơn xin nghỉ cho lớp này trong ngày này rồi.");

            var leaveRequest = new LeaveRequest(
                student.Id,
                request.CourseId,
                request.LeaveDate,
                request.Reason
            );

            _context.LeaveRequests.Add(leaveRequest);

            // Gửi thông báo cho giảng viên phụ trách lớp nếu lớp đã có giảng viên
            if (course.TeacherId.HasValue)
            {
                var notification = new Notification(
                    course.TeacherId.Value,
                    "Có đơn xin nghỉ mới",
                    $"Học viên {student.User.FullName} đã gửi đơn xin nghỉ lớp \"{course.Name}\" ngày {request.LeaveDate:dd/MM/yyyy}.",
                    NotificationType.LeaveRequest,
                    leaveRequest.Id,
                    "LeaveRequest"
                );

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(leaveRequest.Id);
        }
    }
}