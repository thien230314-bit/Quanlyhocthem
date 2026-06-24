using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetMyTuitionInvoices
{
    // Query để học viên xem học phí của mình
    public class GetMyTuitionInvoicesQuery : IRequest<Result<List<MyTuitionInvoiceDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Lọc theo trạng thái hóa đơn, có thể null
        public TuitionInvoiceStatus? Status { get; set; }
    }
}