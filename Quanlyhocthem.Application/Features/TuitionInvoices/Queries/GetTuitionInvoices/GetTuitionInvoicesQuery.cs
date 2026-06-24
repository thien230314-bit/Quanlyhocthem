using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoices
{
    // Query để Admin xem danh sách hóa đơn học phí
    public class GetTuitionInvoicesQuery : IRequest<Result<List<TuitionInvoiceDto>>>
    {
        // Lọc theo học viên, có thể null
        public Guid? StudentId { get; set; }

        // Lọc theo lớp học, có thể null
        public Guid? CourseId { get; set; }

        // Lọc theo trạng thái, có thể null
        public TuitionInvoiceStatus? Status { get; set; }
    }
}