using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrollById;
using Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrolls;
using Quanlyhocthem.Domain.Enums;
using System.Security.Claims;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller bảng lương cho giảng viên
    [ApiController]
    [Route("api/teacher/payrolls")]
    [Authorize(Roles = "Teacher")]
    public class TeacherPayrollsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherPayrollsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/teacher/payrolls
        // Giảng viên xem danh sách bảng lương của mình
        [HttpGet]
        public async Task<IActionResult> GetMyPayrolls(
            [FromQuery] int? month,
            [FromQuery] int? year,
            [FromQuery] PayrollStatus? status)
        {
            var result = await _mediator.Send(new GetTeacherPayrollsQuery
            {
                TeacherUserId = GetCurrentUserId(),
                Month = month,
                Year = year,
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
                message = "Lấy danh sách bảng lương thành công.",
                data = result.Data
            });
        }

        // GET: /api/teacher/payrolls/{payrollId}
        // Giảng viên xem chi tiết bảng lương của mình
        [HttpGet("{payrollId}")]
        public async Task<IActionResult> GetMyPayrollById(Guid payrollId)
        {
            var result = await _mediator.Send(new GetTeacherPayrollByIdQuery
            {
                TeacherUserId = GetCurrentUserId(),
                PayrollId = payrollId
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
                message = "Lấy chi tiết bảng lương thành công.",
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