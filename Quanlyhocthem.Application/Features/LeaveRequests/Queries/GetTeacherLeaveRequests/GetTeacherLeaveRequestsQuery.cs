using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetTeacherLeaveRequests
{
    // Query để giảng viên xem danh sách đơn xin nghỉ của các lớp mình phụ trách
    public class GetTeacherLeaveRequestsQuery : IRequest<Result<List<TeacherLeaveRequestDto>>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Lọc theo trạng thái, có thể null
        public LeaveRequestStatus? Status { get; set; }
    }
}