using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoiceById
{
    // DTO chi tiết hóa đơn học phí
    public class TuitionInvoiceDetailDto
    {
        public Guid TuitionInvoiceId { get; set; }

        public Guid StudentId { get; set; }

        public string StudentCode { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public TuitionInvoiceStatus Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}