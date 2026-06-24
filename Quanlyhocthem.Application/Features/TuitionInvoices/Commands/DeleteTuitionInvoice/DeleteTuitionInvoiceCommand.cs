using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.DeleteTuitionInvoice
{
    // Command để Admin xóa hóa đơn học phí
    public class DeleteTuitionInvoiceCommand : IRequest<Result>
    {
        // Id hóa đơn học phí cần xóa
        public Guid TuitionInvoiceId { get; set; }
    }
}