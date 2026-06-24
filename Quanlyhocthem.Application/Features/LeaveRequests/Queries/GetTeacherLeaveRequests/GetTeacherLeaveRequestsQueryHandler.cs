using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetTeacherLeaveRequests
{
    // Handler lấy danh sách đơn xin nghỉ cho giảng viên
    public class GetTeacherLeaveRequestsQueryHandler : IRequestHandler<GetTeacherLeaveRequestsQuery, Result<List<TeacherLeaveRequestDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherLeaveRequestsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherLeaveRequestDto>>> Handle(GetTeacherLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<TeacherLeaveRequestDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<TeacherLeaveRequestDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<List<TeacherLeaveRequestDto>>.Failure("Tài khoản giảng viên đang bị khóa.");

            var query = _context.LeaveRequests
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .Where(x => x.Course.TeacherId == request.TeacherUserId);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var leaveRequests = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TeacherLeaveRequestDto
                {
                    LeaveRequestId = x.Id,
                    StudentId = x.StudentId,
                    StudentCode = x.Student.StudentCode,
                    StudentName = x.Student.User.FullName,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
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

            return Result<List<TeacherLeaveRequestDto>>.Success(leaveRequests);
        }
    }
}