using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetStudentLeaveRequests
{
    // Handler lấy danh sách đơn xin nghỉ của học viên
    public class GetStudentLeaveRequestsQueryHandler : IRequestHandler<GetStudentLeaveRequestsQuery, Result<List<StudentLeaveRequestDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentLeaveRequestsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentLeaveRequestDto>>> Handle(GetStudentLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<StudentLeaveRequestDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            var query = _context.LeaveRequests
                .Include(x => x.Course)
                .Include(x => x.Teacher)
                .Include(x => x.Admin)
                .Where(x => x.StudentId == student.Id);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var leaveRequests = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new StudentLeaveRequestDto
                {
                    LeaveRequestId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    LeaveDate = x.LeaveDate,
                    Reason = x.Reason,
                    Status = x.Status,
                    TeacherName = x.Teacher != null ? x.Teacher.FullName : null,
                    TeacherReviewedAt = x.TeacherReviewedAt,
                    TeacherNote = x.TeacherNote,
                    AdminName = x.Admin != null ? x.Admin.FullName : null,
                    AdminReviewedAt = x.AdminReviewedAt,
                    AdminNote = x.AdminNote,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<StudentLeaveRequestDto>>.Success(leaveRequests);
        }
    }
}