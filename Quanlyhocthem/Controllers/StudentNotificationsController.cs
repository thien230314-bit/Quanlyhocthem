using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Quanlyhocthem.Application.Features.Notifications.Queries.GetStudentNotifications;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller thông báo cho học viên
    [ApiController]
    [Route("api/student/notifications")]
    [Authorize(Roles = "Student")]
    public class StudentNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentNotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/student/notifications
        // Học viên xem tất cả thông báo của mình
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] bool unreadOnly = false)
        {
            var result = await _mediator.Send(new GetStudentNotificationsQuery
            {
                StudentUserId = GetCurrentUserId(),
                UnreadOnly = unreadOnly
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            var data = result.Data ?? new List<StudentNotificationDto>();

            return Ok(new
            {
                message = "Lấy danh sách thông báo thành công.",
                unreadCount = data.Count(x => !x.IsRead),
                data
            });
        }

        // PATCH: /api/student/notifications/{notificationId}/read
        // Học viên đánh dấu một thông báo là đã đọc
        [HttpPatch("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var result = await _mediator.Send(new MarkNotificationAsReadCommand
            {
                StudentUserId = GetCurrentUserId(),
                NotificationId = notificationId
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
                message = "Đã đánh dấu thông báo là đã đọc."
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