using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoiceById
{
    // Query lấy chi tiết một hóa đơn học phí
    public class GetTuitionInvoiceByIdQuery : IRequest<Result<TuitionInvoiceDetailDto>>
    {
        // Id hóa đơn học phí
        public Guid TuitionInvoiceId { get; set; }
    }
}