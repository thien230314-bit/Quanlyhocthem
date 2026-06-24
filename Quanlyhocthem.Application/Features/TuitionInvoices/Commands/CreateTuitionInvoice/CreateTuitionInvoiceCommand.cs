using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.CreateTuitionInvoice
{
    // Command để Admin tạo hóa đơn học phí
    public class CreateTuitionInvoiceCommand : IRequest<Result<Guid>>
    {
        // Id học viên
        public Guid StudentId { get; set; }

        // Id lớp học
        public Guid CourseId { get; set; }

        // Số tiền học phí
        public decimal Amount { get; set; }

        // Hạn đóng học phí
        public DateTime DueDate { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}