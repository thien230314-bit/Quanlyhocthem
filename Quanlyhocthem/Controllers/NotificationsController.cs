using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Quanlyhocthem.Application.Features.Notifications.Queries.GetMyNotifications;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller thông báo dùng chung cho Admin, Teacher, Student
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/notifications
        // Người dùng hiện tại xem thông báo của mình
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] bool unreadOnly = false)
        {
            var result = await _mediator.Send(new GetMyNotificationsQuery
            {
                CurrentUserId = GetCurrentUserId(),
                UnreadOnly = unreadOnly
            });

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = result.Error
                });
            }

            var data = result.Data ?? new List<NotificationDto>();

            return Ok(new
            {
                message = "Lấy danh sách thông báo thành công.",
                unreadCount = data.Count(x => !x.IsRead),
                data
            });
        }

        // PATCH: /api/notifications/{notificationId}/read
        // Người dùng hiện tại đánh dấu thông báo đã đọc
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