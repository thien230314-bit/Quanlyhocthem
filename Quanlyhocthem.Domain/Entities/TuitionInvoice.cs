using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // TuitionInvoice là hóa đơn học phí của học viên theo từng lớp học
    public class TuitionInvoice : BaseEntity
    {
        // Học viên cần đóng học phí
        public Guid StudentId { get; private set; }

        public Student Student { get; private set; } = null!;

        // Lớp học phát sinh học phí
        public Guid CourseId { get; private set; }

        public Course Course { get; private set; } = null!;

        // Số tiền học phí
        public decimal Amount { get; private set; }

        // Hạn đóng học phí
        public DateTime DueDate { get; private set; }

        // Trạng thái hóa đơn
        public TuitionInvoiceStatus Status { get; private set; }

        // Thời gian thanh toán
        public DateTime? PaidAt { get; private set; }

        // Ghi chú
        public string? Note { get; private set; }

        private TuitionInvoice()
        {
        }

        public TuitionInvoice(Guid studentId, Guid courseId, decimal amount, DateTime dueDate, string? note)
        {
            if (studentId == Guid.Empty)
                throw new ArgumentException("Id học viên không hợp lệ.");

            if (courseId == Guid.Empty)
                throw new ArgumentException("Id lớp học không hợp lệ.");

            if (amount <= 0)
                throw new ArgumentException("Số tiền học phí phải lớn hơn 0.");

            StudentId = studentId;
            CourseId = courseId;
            Amount = amount;
            DueDate = dueDate;
            Status = TuitionInvoiceStatus.Unpaid;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        }

        // Cập nhật thông tin hóa đơn khi chưa thanh toán
        public void Update(decimal amount, DateTime dueDate, string? note)
        {
            if (Status == TuitionInvoiceStatus.Paid)
                throw new InvalidOperationException("Không thể cập nhật hóa đơn đã thanh toán.");

            if (amount <= 0)
                throw new ArgumentException("Số tiền học phí phải lớn hơn 0.");

            Amount = amount;
            DueDate = dueDate;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            SetUpdatedAt();
        }

        // Đánh dấu hóa đơn đã thanh toán
        public void MarkAsPaid(DateTime paidAt)
        {
            if (Status == TuitionInvoiceStatus.Cancelled)
                throw new InvalidOperationException("Không thể thanh toán hóa đơn đã hủy.");

            Status = TuitionInvoiceStatus.Paid;
            PaidAt = paidAt;

            SetUpdatedAt();
        }

        // Đánh dấu hóa đơn quá hạn
        public void MarkAsOverdue()
        {
            if (Status == TuitionInvoiceStatus.Unpaid && DueDate < DateTime.UtcNow)
            {
                Status = TuitionInvoiceStatus.Overdue;
                SetUpdatedAt();
            }
        }

        // Hủy hóa đơn
        public void Cancel()
        {
            if (Status == TuitionInvoiceStatus.Paid)
                throw new InvalidOperationException("Không thể hủy hóa đơn đã thanh toán.");

            Status = TuitionInvoiceStatus.Cancelled;

            SetUpdatedAt();
        }
    }
}