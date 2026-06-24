using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest
{
    // Command để học viên sửa đơn xin nghỉ
    public class UpdateLeaveRequestCommand : IRequest<Result>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id đơn xin nghỉ cần sửa
        public Guid LeaveRequestId { get; set; }

        // Ngày xin nghỉ mới
        public DateTime LeaveDate { get; set; }

        // Lý do mới
        public string Reason { get; set; } = string.Empty;
    }
}