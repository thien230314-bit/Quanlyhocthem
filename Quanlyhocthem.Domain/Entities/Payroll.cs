using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Payroll là bảng lương theo tháng của giảng viên
    public class Payroll : BaseEntity
    {
        // Giảng viên được tính lương
        public Guid TeacherId { get; private set; }

        public User Teacher { get; private set; } = null!;

        // Tháng tính lương
        public int Month { get; private set; }

        // Năm tính lương
        public int Year { get; private set; }

        // Số buổi dạy được hệ thống ghi nhận
        public int TeachingSessions { get; private set; }

        // Lương cơ bản
        public decimal BaseSalary { get; private set; }

        // Tiền mỗi buổi dạy
        public decimal TeachingSessionAmount { get; private set; }

        // Thưởng thêm
        public decimal BonusAmount { get; private set; }

        // Khoản trừ
        public decimal DeductionAmount { get; private set; }

        // Tổng lương cuối cùng
        public decimal TotalAmount { get; private set; }

        // Trạng thái bảng lương
        public PayrollStatus Status { get; private set; }

        // Ngày thanh toán
        public DateTime? PaidAt { get; private set; }

        // Ghi chú
        public string? Note { get; private set; }

        private Payroll()
        {
        }

        public Payroll(
            Guid teacherId,
            int month,
            int year,
            int teachingSessions,
            decimal baseSalary,
            decimal teachingSessionAmount,
            decimal bonusAmount,
            decimal deductionAmount,
            string? note)
        {
            if (teacherId == Guid.Empty)
                throw new ArgumentException("Id giảng viên không hợp lệ.");

            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng tính lương không hợp lệ.");

            if (year < 2000)
                throw new ArgumentException("Năm tính lương không hợp lệ.");

            if (teachingSessions < 0)
                throw new ArgumentException("Số buổi dạy không được âm.");

            if (baseSalary < 0)
                throw new ArgumentException("Lương cơ bản không được âm.");

            if (teachingSessionAmount < 0)
                throw new ArgumentException("Tiền mỗi buổi dạy không được âm.");

            if (bonusAmount < 0)
                throw new ArgumentException("Tiền thưởng không được âm.");

            if (deductionAmount < 0)
                throw new ArgumentException("Tiền trừ không được âm.");

            TeacherId = teacherId;
            Month = month;
            Year = year;
            TeachingSessions = teachingSessions;
            BaseSalary = baseSalary;
            TeachingSessionAmount = teachingSessionAmount;
            BonusAmount = bonusAmount;
            DeductionAmount = deductionAmount;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            Status = PayrollStatus.Draft;

            RecalculateTotalAmount();
        }

        // Cập nhật bảng lương khi chưa thanh toán
        public void Update(
            int teachingSessions,
            decimal baseSalary,
            decimal teachingSessionAmount,
            decimal bonusAmount,
            decimal deductionAmount,
            string? note)
        {
            if (Status == PayrollStatus.Paid)
                throw new InvalidOperationException("Không thể cập nhật bảng lương đã thanh toán.");

            if (Status == PayrollStatus.Cancelled)
                throw new InvalidOperationException("Không thể cập nhật bảng lương đã hủy.");

            if (teachingSessions < 0)
                throw new ArgumentException("Số buổi dạy không được âm.");

            if (baseSalary < 0)
                throw new ArgumentException("Lương cơ bản không được âm.");

            if (teachingSessionAmount < 0)
                throw new ArgumentException("Tiền mỗi buổi dạy không được âm.");

            if (bonusAmount < 0)
                throw new ArgumentException("Tiền thưởng không được âm.");

            if (deductionAmount < 0)
                throw new ArgumentException("Tiền trừ không được âm.");

            TeachingSessions = teachingSessions;
            BaseSalary = baseSalary;
            TeachingSessionAmount = teachingSessionAmount;
            BonusAmount = bonusAmount;
            DeductionAmount = deductionAmount;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            RecalculateTotalAmount();
            SetUpdatedAt();
        }

        // Xác nhận bảng lương
        public void Confirm()
        {
            if (Status == PayrollStatus.Paid)
                throw new InvalidOperationException("Bảng lương đã thanh toán.");

            if (Status == PayrollStatus.Cancelled)
                throw new InvalidOperationException("Không thể xác nhận bảng lương đã hủy.");

            Status = PayrollStatus.Confirmed;
            SetUpdatedAt();
        }

        // Đánh dấu đã thanh toán
        public void MarkAsPaid(DateTime paidAt)
        {
            if (Status == PayrollStatus.Cancelled)
                throw new InvalidOperationException("Không thể thanh toán bảng lương đã hủy.");

            Status = PayrollStatus.Paid;
            PaidAt = paidAt;
            SetUpdatedAt();
        }

        // Hủy bảng lương
        public void Cancel()
        {
            if (Status == PayrollStatus.Paid)
                throw new InvalidOperationException("Không thể hủy bảng lương đã thanh toán.");

            Status = PayrollStatus.Cancelled;
            SetUpdatedAt();
        }

        // Tính tổng lương
        private void RecalculateTotalAmount()
        {
            TotalAmount = BaseSalary + (TeachingSessions * TeachingSessionAmount) + BonusAmount - DeductionAmount;

            if (TotalAmount < 0)
                TotalAmount = 0;
        }
    }
}