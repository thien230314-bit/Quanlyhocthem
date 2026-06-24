using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.UpdatePayroll
{
    // Command để Admin cập nhật bảng lương
    public class UpdatePayrollCommand : IRequest<Result>
    {
        // Id bảng lương
        public Guid PayrollId { get; set; }

        // Số buổi dạy
        public int TeachingSessions { get; set; }

        // Lương cơ bản
        public decimal BaseSalary { get; set; }

        // Tiền mỗi buổi dạy
        public decimal TeachingSessionAmount { get; set; }

        // Tiền thưởng
        public decimal BonusAmount { get; set; }

        // Tiền trừ
        public decimal DeductionAmount { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}