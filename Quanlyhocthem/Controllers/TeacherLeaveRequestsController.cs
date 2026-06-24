using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.TeacherApproveLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.TeacherRejectLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetTeacherLeaveRequests;
using Quanlyhocthem.Domain.Enums;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller đơn xin nghỉ cho giảng viên
    [ApiController]
    [Route("api/teacher/leave-requests")]
    [Authorize(Roles = "Teacher")]
    public class TeacherLeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherLeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/teacher/leave-requests
        // Giảng viên xem danh sách đơn xin nghỉ của các lớp mình phụ trách
        [HttpGet]
        public async Task<IActionResult> GetTeacherLeaveRequests([FromQuery] LeaveRequestStatus? status)
        {
            var result = await _mediator.Send(new GetTeacherLeaveRequestsQuery
            {
                TeacherUserId = GetCurrentUserId(),
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

        // GET: /api/teacher/leave-requests/{leaveRequestId}
        // Giảng viên xem chi tiết một đơn xin nghỉ
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

        // PATCH: /api/teacher/leave-requests/{leaveRequestId}/approve
        // Giảng viên duyệt đơn xin nghỉ
        [HttpPatch("{leaveRequestId}/approve")]
        public async Task<IActionResult> ApproveLeaveRequest(Guid leaveRequestId, [FromBody] TeacherApproveLeaveRequestCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();
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
                message = "Duyệt đơn xin nghỉ thành công."
            });
        }

        // PATCH: /api/teacher/leave-requests/{leaveRequestId}/reject
        // Giảng viên từ chối đơn xin nghỉ
        [HttpPatch("{leaveRequestId}/reject")]
        public async Task<IActionResult> RejectLeaveRequest(Guid leaveRequestId, [FromBody] TeacherRejectLeaveRequestCommand command)
        {
            command.TeacherUserId = GetCurrentUserId();
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
                message = "Từ chối đơn xin nghỉ thành công."
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