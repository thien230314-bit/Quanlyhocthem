using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetMyTuitionInvoices
{
    // DTO học viên xem hóa đơn học phí của mình
    public class MyTuitionInvoiceDto
    {
        public Guid TuitionInvoiceId { get; set; }

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public TuitionInvoiceStatus Status { get; set; }

        public bool IsOverdue { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}