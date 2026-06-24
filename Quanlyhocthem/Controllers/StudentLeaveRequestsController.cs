using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetStudentLeaveRequests;
using Quanlyhocthem.Domain.Enums;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller đơn xin nghỉ cho học viên
    [ApiController]
    [Route("api/student/leave-requests")]
    [Authorize(Roles = "Student")]
    public class StudentLeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentLeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/student/leave-requests
        // Học viên xem danh sách đơn xin nghỉ của mình
        [HttpGet]
        public async Task<IActionResult> GetMyLeaveRequests([FromQuery] LeaveRequestStatus? status)
        {
            var result = await _mediator.Send(new GetStudentLeaveRequestsQuery
            {
                StudentUserId = GetCurrentUserId(),
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

        // GET: /api/student/leave-requests/{leaveRequestId}
        // Học viên xem chi tiết đơn xin nghỉ của mình
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

        // POST: /api/student/leave-requests
        // Học viên gửi đơn xin nghỉ
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestCommand command)
        {
            command.StudentUserId = GetCurrentUserId();

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
                message = "Gửi đơn xin nghỉ thành công.",
                leaveRequestId = result.Data
            });
        }

        // PUT: /api/student/leave-requests/{leaveRequestId}
        // Học viên sửa đơn xin nghỉ khi đơn chưa được duyệt
        [HttpPut("{leaveRequestId}")]
        public async Task<IActionResult> UpdateLeaveRequest(Guid leaveRequestId, [FromBody] UpdateLeaveRequestCommand command)
        {
            command.StudentUserId = GetCurrentUserId();
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
                message = "Cập nhật đơn xin nghỉ thành công."
            });
        }

        // DELETE: /api/student/leave-requests/{leaveRequestId}
        // Học viên xóa đơn xin nghỉ khi đơn chưa được duyệt
        [HttpDelete("{leaveRequestId}")]
        public async Task<IActionResult> DeleteLeaveRequest(Guid leaveRequestId)
        {
            var result = await _mediator.Send(new DeleteLeaveRequestCommand
            {
                StudentUserId = GetCurrentUserId(),
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
                message = "Xóa đơn xin nghỉ thành công."
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