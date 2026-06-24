using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById
{
    // Query lấy chi tiết một đơn xin nghỉ
    public class GetLeaveRequestByIdQuery : IRequest<Result<LeaveRequestDetailDto>>
    {
        // Id User hiện tại, lấy từ JWT token
        public Guid CurrentUserId { get; set; }

        // Id đơn xin nghỉ
        public Guid LeaveRequestId { get; set; }
    }
}