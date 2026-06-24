using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyhocthem.Application.Features.TuitionInvoices.Commands.CreateTuitionInvoice;
using Quanlyhocthem.Application.Features.TuitionInvoices.Commands.DeleteTuitionInvoice;
using Quanlyhocthem.Application.Features.TuitionInvoices.Commands.MarkTuitionInvoiceAsPaid;
using Quanlyhocthem.Application.Features.TuitionInvoices.Commands.UpdateTuitionInvoice;
using Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoiceById;
using Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoices;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Api.Controllers
{
    // Controller quản lý hóa đơn học phí
    [ApiController]
    [Route("api/tuition-invoices")]
    [Authorize(Roles = "Admin")]
    public class TuitionInvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TuitionInvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /api/tuition-invoices
        // Admin xem danh sách hóa đơn học phí
        [HttpGet]
        public async Task<IActionResult> GetTuitionInvoices(
            [FromQuery] Guid? studentId,
            [FromQuery] Guid? courseId,
            [FromQuery] TuitionInvoiceStatus? status)
        {
            var result = await _mediator.Send(new GetTuitionInvoicesQuery
            {
                StudentId = studentId,
                CourseId = courseId,
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
                message = "Lấy danh sách hóa đơn học phí thành công.",
                data = result.Data
            });
        }

        // GET: /api/tuition-invoices/{tuitionInvoiceId}
        // Admin xem chi tiết hóa đơn học phí
        [HttpGet("{tuitionInvoiceId}")]
        public async Task<IActionResult> GetTuitionInvoiceById(Guid tuitionInvoiceId)
        {
            var result = await _mediator.Send(new GetTuitionInvoiceByIdQuery
            {
                TuitionInvoiceId = tuitionInvoiceId
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
                message = "Lấy chi tiết hóa đơn học phí thành công.",
                data = result.Data
            });
        }

        // POST: /api/tuition-invoices
        // Admin tạo hóa đơn học phí
        [HttpPost]
        public async Task<IActionResult> CreateTuitionInvoice([FromBody] CreateTuitionInvoiceCommand command)
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
                message = "Tạo hóa đơn học phí thành công.",
                tuitionInvoiceId = result.Data
            });
        }

        // PUT: /api/tuition-invoices/{tuitionInvoiceId}
        // Admin cập nhật hóa đơn học phí
        [HttpPut("{tuitionInvoiceId}")]
        public async Task<IActionResult> UpdateTuitionInvoice(Guid tuitionInvoiceId, [FromBody] UpdateTuitionInvoiceCommand command)
        {
            command.TuitionInvoiceId = tuitionInvoiceId;

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
                message = "Cập nhật hóa đơn học phí thành công."
            });
        }

        // PATCH: /api/tuition-invoices/{tuitionInvoiceId}/paid
        // Admin xác nhận hóa đơn đã thanh toán
        [HttpPatch("{tuitionInvoiceId}/paid")]
        public async Task<IActionResult> MarkAsPaid(Guid tuitionInvoiceId, [FromBody] MarkTuitionInvoiceAsPaidCommand command)
        {
            command.TuitionInvoiceId = tuitionInvoiceId;

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
                message = "Xác nhận thanh toán học phí thành công."
            });
        }

        // DELETE: /api/tuition-invoices/{tuitionInvoiceId}
        // Admin xóa hóa đơn học phí nếu chưa thanh toán
        [HttpDelete("{tuitionInvoiceId}")]
        public async Task<IActionResult> DeleteTuitionInvoice(Guid tuitionInvoiceId)
        {
            var result = await _mediator.Send(new DeleteTuitionInvoiceCommand
            {
                TuitionInvoiceId = tuitionInvoiceId
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
                message = "Xóa hóa đơn học phí thành công."
            });
        }
    }
}