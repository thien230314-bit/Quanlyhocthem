using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetStudentLeaveRequests
{
    // Query để học viên xem danh sách đơn xin nghỉ của mình
    public class GetStudentLeaveRequestsQuery : IRequest<Result<List<StudentLeaveRequestDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Lọc theo trạng thái, có thể null
        public LeaveRequestStatus? Status { get; set; }
    }
}