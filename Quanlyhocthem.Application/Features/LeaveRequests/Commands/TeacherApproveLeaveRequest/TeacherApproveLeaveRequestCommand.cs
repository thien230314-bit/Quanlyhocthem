using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.TeacherApproveLeaveRequest
{
    // Command để giảng viên duyệt đơn xin nghỉ
    public class TeacherApproveLeaveRequestCommand : IRequest<Result>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id đơn xin nghỉ
        public Guid LeaveRequestId { get; set; }

        // Ghi chú của giảng viên
        public string? TeacherNote { get; set; }
    }
}