using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetAdminLeaveRequests
{
    // Query để Admin xem danh sách đơn xin nghỉ
    public class GetAdminLeaveRequestsQuery : IRequest<Result<List<AdminLeaveRequestDto>>>
    {
        // Id User của Admin, lấy từ JWT token
        public Guid AdminUserId { get; set; }

        // Lọc theo trạng thái, có thể null
        public LeaveRequestStatus? Status { get; set; }
    }
}