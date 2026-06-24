using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById
{
    // Handler lấy chi tiết đơn xin nghỉ
    public class GetLeaveRequestByIdQueryHandler : IRequestHandler<GetLeaveRequestByIdQuery, Result<LeaveRequestDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetLeaveRequestByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<LeaveRequestDetailDto>> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeaveRequestId == Guid.Empty)
                return Result<LeaveRequestDetailDto>.Failure("Id đơn xin nghỉ không hợp lệ.");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.CurrentUserId, cancellationToken);

            if (user == null)
                return Result<LeaveRequestDetailDto>.Failure("Không tìm thấy tài khoản người dùng.");

            var leaveRequest = await _context.LeaveRequests
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .Include(x => x.Admin)
                .FirstOrDefaultAsync(x => x.Id == request.LeaveRequestId, cancellationToken);

            if (leaveRequest == null)
                return Result<LeaveRequestDetailDto>.Failure("Không tìm thấy đơn xin nghỉ.");

            // Admin được xem tất cả đơn
            if (user.Role == Role.Admin)
                return Result<LeaveRequestDetailDto>.Success(MapToDto(leaveRequest));

            // Teacher chỉ xem đơn của lớp mình phụ trách
            if (user.Role == Role.Teacher)
            {
                if (leaveRequest.Course.TeacherId != request.CurrentUserId)
                    return Result<LeaveRequestDetailDto>.Failure("Bạn không có quyền xem đơn xin nghỉ này.");

                return Result<LeaveRequestDetailDto>.Success(MapToDto(leaveRequest));
            }

            // Student chỉ xem đơn của chính mình
            if (user.Role == Role.Student)
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(x => x.UserId == request.CurrentUserId, cancellationToken);

                if (student == null)
                    return Result<LeaveRequestDetailDto>.Failure("Không tìm thấy hồ sơ học viên.");

                if (leaveRequest.StudentId != student.Id)
                    return Result<LeaveRequestDetailDto>.Failure("Bạn không có quyền xem đơn xin nghỉ này.");

                return Result<LeaveRequestDetailDto>.Success(MapToDto(leaveRequest));
            }

            return Result<LeaveRequestDetailDto>.Failure("Vai trò không hợp lệ.");
        }

        private static LeaveRequestDetailDto MapToDto(Domain.Entities.LeaveRequest leaveRequest)
        {
            return new LeaveRequestDetailDto
            {
                LeaveRequestId = leaveRequest.Id,
                StudentId = leaveRequest.StudentId,
                StudentCode = leaveRequest.Student.StudentCode,
                StudentName = leaveRequest.Student.User.FullName,
                CourseId = leaveRequest.CourseId,
                CourseName = leaveRequest.Course.Name,
                LeaveDate = leaveRequest.LeaveDate,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Status,
                TeacherId = leaveRequest.TeacherId,
                TeacherName = leaveRequest.Teacher != null ? leaveRequest.Teacher.FullName : null,
                TeacherReviewedAt = leaveRequest.TeacherReviewedAt,
                TeacherNote = leaveRequest.TeacherNote,
                AdminId = leaveRequest.AdminId,
                AdminName = leaveRequest.Admin != null ? leaveRequest.Admin.FullName : null,
                AdminReviewedAt = leaveRequest.AdminReviewedAt,
                AdminNote = leaveRequest.AdminNote,
                CreatedAt = leaveRequest.CreatedAt,
                UpdatedAt = leaveRequest.UpdatedAt
            };
        }
    }
}