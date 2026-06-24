using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.MarkTuitionInvoiceAsPaid
{
    // Command để Admin xác nhận hóa đơn đã thanh toán
    public class MarkTuitionInvoiceAsPaidCommand : IRequest<Result>
    {
        // Id hóa đơn học phí
        public Guid TuitionInvoiceId { get; set; }

        // Ngày thanh toán, nếu null thì lấy thời gian hiện tại
        public DateTime? PaidAt { get; set; }
    }
}