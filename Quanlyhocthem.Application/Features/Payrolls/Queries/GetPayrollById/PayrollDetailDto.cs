using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrollById
{
    // DTO chi tiết bảng lương
    public class PayrollDetailDto
    {
        public Guid PayrollId { get; set; }

        public Guid TeacherId { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public string TeacherEmail { get; set; } = string.Empty;

        public int Month { get; set; }

        public int Year { get; set; }

        public int TeachingSessions { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal TeachingSessionAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public decimal DeductionAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public PayrollStatus Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}