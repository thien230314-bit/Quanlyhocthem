using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest
{
    // Command để học viên xóa đơn xin nghỉ
    public class DeleteLeaveRequestCommand : IRequest<Result>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id đơn xin nghỉ cần xóa
        public Guid LeaveRequestId { get; set; }
    }
}