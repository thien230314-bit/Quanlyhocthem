using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.CreatePayroll
{
    // Command để Admin tạo bảng lương cho giảng viên
    public class CreatePayrollCommand : IRequest<Result<Guid>>
    {
        // Id giảng viên
        public Guid TeacherId { get; set; }

        // Tháng tính lương
        public int Month { get; set; }

        // Năm tính lương
        public int Year { get; set; }

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