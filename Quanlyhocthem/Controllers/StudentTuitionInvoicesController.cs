using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetMyTuitionInvoices;
using Quanlyhocthem.Domain.Enums;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller học phí cho học viên
    [ApiController]
    [Route("api/student/tuition-invoices")]
    [Authorize(Roles = "Student")]
    public class StudentTuitionInvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentTuitionInvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/student/tuition-invoices
        // Học viên xem danh sách hóa đơn học phí của mình
        [HttpGet]
        public async Task<IActionResult> GetMyTuitionInvoices([FromQuery] TuitionInvoiceStatus? status)
        {
            var result = await _mediator.Send(new GetMyTuitionInvoicesQuery
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
                message = "Lấy danh sách học phí thành công.",
                data = result.Data
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