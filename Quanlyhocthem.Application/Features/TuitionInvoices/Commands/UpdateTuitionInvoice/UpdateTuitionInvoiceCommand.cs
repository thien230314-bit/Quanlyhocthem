using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.UpdateTuitionInvoice
{
    // Command để Admin cập nhật hóa đơn học phí
    public class UpdateTuitionInvoiceCommand : IRequest<Result>
    {
        // Id hóa đơn học phí cần cập nhật
        public Guid TuitionInvoiceId { get; set; }

        // Số tiền học phí
        public decimal Amount { get; set; }

        // Hạn đóng học phí
        public DateTime DueDate { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}