using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    // Command để học viên gửi đơn xin nghỉ
    public class CreateLeaveRequestCommand : IRequest<Result<Guid>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id lớp học xin nghỉ
        public Guid CourseId { get; set; }

        // Ngày xin nghỉ
        public DateTime LeaveDate { get; set; }

        // Lý do xin nghỉ
        public string Reason { get; set; } = string.Empty;
    }
}