using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetAdminLeaveRequests
{
    // Handler lấy danh sách đơn xin nghỉ cho Admin
    public class GetAdminLeaveRequestsQueryHandler : IRequestHandler<GetAdminLeaveRequestsQuery, Result<List<AdminLeaveRequestDto>>>
    {
        private readonly IAppDbContext _context;

        public GetAdminLeaveRequestsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AdminLeaveRequestDto>>> Handle(GetAdminLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var admin = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.AdminUserId, cancellationToken);

            if (admin == null)
                return Result<List<AdminLeaveRequestDto>>.Failure("Không tìm thấy tài khoản Admin.");

            if (admin.Role != Role.Admin)
                return Result<List<AdminLeaveRequestDto>>.Failure("Tài khoản hiện tại không phải Admin.");

            if (!admin.IsActive)
                return Result<List<AdminLeaveRequestDto>>.Failure("Tài khoản Admin đang bị khóa.");

            var query = _context.LeaveRequests
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .Where(x =>
                    x.Status == LeaveRequestStatus.ApprovedByTeacher ||
                    x.Status == LeaveRequestStatus.ApprovedByAdmin ||
                    x.Status == LeaveRequestStatus.RejectedByAdmin);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var leaveRequests = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminLeaveRequestDto
                {
                    LeaveRequestId = x.Id,
                    StudentId = x.StudentId,
                    StudentCode = x.Student.StudentCode,
                    StudentName = x.Student.User.FullName,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    TeacherId = x.TeacherId,
                    TeacherName = x.Teacher != null ? x.Teacher.FullName : null,
                    LeaveDate = x.LeaveDate,
                    Reason = x.Reason,
                    Status = x.Status,
                    TeacherReviewedAt = x.TeacherReviewedAt,
                    TeacherNote = x.TeacherNote,
                    AdminReviewedAt = x.AdminReviewedAt,
                    AdminNote = x.AdminNote,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<AdminLeaveRequestDto>>.Success(leaveRequests);
        }
    }
}