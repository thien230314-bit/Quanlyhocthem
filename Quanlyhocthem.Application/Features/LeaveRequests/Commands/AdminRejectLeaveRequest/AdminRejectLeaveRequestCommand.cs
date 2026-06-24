using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.AdminRejectLeaveRequest
{
    // Command để Admin từ chối đơn xin nghỉ đã được giảng viên duyệt
    public class AdminRejectLeaveRequestCommand : IRequest<Result>
    {
        // Id User của Admin, lấy từ JWT token
        public Guid AdminUserId { get; set; }

        // Id đơn xin nghỉ
        public Guid LeaveRequestId { get; set; }

        // Ghi chú của Admin
        public string? AdminNote { get; set; }
    }
}