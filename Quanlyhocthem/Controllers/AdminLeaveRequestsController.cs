using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.AdminApproveLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.AdminRejectLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetAdminLeaveRequests;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using Quanlyhocthem.Domain.Enums;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller đơn xin nghỉ cho Admin
    [ApiController]
    [Route("api/admin/leave-requests")]
    [Authorize(Roles = "Admin")]
    public class AdminLeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminLeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/admin/leave-requests
        // Admin xem danh sách đơn xin nghỉ đã qua giảng viên
        [HttpGet]
        public async Task<IActionResult> GetAdminLeaveRequests([FromQuery] LeaveRequestStatus? status)
        {
            var result = await _mediator.Send(new GetAdminLeaveRequestsQuery
            {
                AdminUserId = GetCurrentUserId(),
                Status = status
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy danh sách đơn xin nghỉ thành công.",
                data = result.Data
            });
        }

        // GET: /api/admin/leave-requests/{leaveRequestId}
        // Admin xem chi tiết một đơn xin nghỉ
        [HttpGet("{leaveRequestId}")]
        public async Task<IActionResult> GetLeaveRequestById(Guid leaveRequestId)
        {
            var result = await _mediator.Send(new GetLeaveRequestByIdQuery
            {
                CurrentUserId = GetCurrentUserId(),
                LeaveRequestId = leaveRequestId
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Lấy chi tiết đơn xin nghỉ thành công.",
                data = result.Data
            });
        }

        // PATCH: /api/admin/leave-requests/{leaveRequestId}/approve
        // Admin xác nhận đơn xin nghỉ đã được giảng viên duyệt
        [HttpPatch("{leaveRequestId}/approve")]
        public async Task<IActionResult> ApproveLeaveRequest(Guid leaveRequestId, [FromBody] AdminApproveLeaveRequestCommand command)
        {
            command.AdminUserId = GetCurrentUserId();
            command.LeaveRequestId = leaveRequestId;

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Admin xác nhận đơn xin nghỉ thành công."
            });
        }

        // PATCH: /api/admin/leave-requests/{leaveRequestId}/reject
        // Admin từ chối đơn xin nghỉ đã được giảng viên duyệt
        [HttpPatch("{leaveRequestId}/reject")]
        public async Task<IActionResult> RejectLeaveRequest(Guid leaveRequestId, [FromBody] AdminRejectLeaveRequestCommand command)
        {
            command.AdminUserId = GetCurrentUserId();
            command.LeaveRequestId = leaveRequestId;

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            return Ok(new
            {
                message = "Admin từ chối đơn xin nghỉ thành công."
            });
        }

        // Lấy UserId từ JWT token
        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Không tìm thấy UserId trong token.");

            return Guid.Parse(userId);
        }
    }
}