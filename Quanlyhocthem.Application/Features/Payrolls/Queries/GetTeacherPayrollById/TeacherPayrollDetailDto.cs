using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrollById
{
    // DTO chi tiết bảng lương cho giảng viên
    public class TeacherPayrollDetailDto
    {
        public Guid PayrollId { get; set; }

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