using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.Payrolls.Commands.ConfirmPayroll;
using Quanlyhocthem.Application.Features.Payrolls.Commands.CreatePayroll;
using Quanlyhocthem.Application.Features.Payrolls.Commands.DeletePayroll;
using Quanlyhocthem.Application.Features.Payrolls.Commands.MarkPayrollAsPaid;
using Quanlyhocthem.Application.Features.Payrolls.Commands.UpdatePayroll;
using Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrollById;
using Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrolls;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý bảng lương giảng viên
    [ApiController]
    [Route("api/payrolls")]
    [Authorize(Roles = "Admin")]
    public class PayrollsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/payrolls
        // Admin xem danh sách bảng lương
        [HttpGet]
        public async Task<IActionResult> GetPayrolls(
            [FromQuery] Guid? teacherId,
            [FromQuery] int? month,
            [FromQuery] int? year,
            [FromQuery] PayrollStatus? status)
        {
            var result = await _mediator.Send(new GetPayrollsQuery
            {
                TeacherId = teacherId,
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

        // GET: /api/payrolls/{payrollId}
        // Admin xem chi tiết bảng lương
        [HttpGet("{payrollId}")]
        public async Task<IActionResult> GetPayrollById(Guid payrollId)
        {
            var result = await _mediator.Send(new GetPayrollByIdQuery
            {
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

        // POST: /api/payrolls
        // Admin tạo bảng lương
        [HttpPost]
        public async Task<IActionResult> CreatePayroll([FromBody] CreatePayrollCommand command)
        {
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
                message = "Tạo bảng lương thành công.",
                payrollId = result.Data
            });
        }

        // PUT: /api/payrolls/{payrollId}
        // Admin cập nhật bảng lương
        [HttpPut("{payrollId}")]
        public async Task<IActionResult> UpdatePayroll(Guid payrollId, [FromBody] UpdatePayrollCommand command)
        {
            command.PayrollId = payrollId;

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
                message = "Cập nhật bảng lương thành công."
            });
        }

        // PATCH: /api/payrolls/{payrollId}/confirm
        // Admin xác nhận bảng lương
        [HttpPatch("{payrollId}/confirm")]
        public async Task<IActionResult> ConfirmPayroll(Guid payrollId)
        {
            var result = await _mediator.Send(new ConfirmPayrollCommand
            {
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
                message = "Xác nhận bảng lương thành công."
            });
        }

        // PATCH: /api/payrolls/{payrollId}/paid
        // Admin đánh dấu bảng lương đã thanh toán
        [HttpPatch("{payrollId}/paid")]
        public async Task<IActionResult> MarkPayrollAsPaid(Guid payrollId, [FromBody] MarkPayrollAsPaidCommand command)
        {
            command.PayrollId = payrollId;

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
                message = "Xác nhận thanh toán bảng lương thành công."
            });
        }

        // DELETE: /api/payrolls/{payrollId}
        // Admin xóa bảng lương nếu chưa thanh toán
        [HttpDelete("{payrollId}")]
        public async Task<IActionResult> DeletePayroll(Guid payrollId)
        {
            var result = await _mediator.Send(new DeletePayrollCommand
            {
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
                message = "Xóa bảng lương thành công."
            });
        }
    }
}