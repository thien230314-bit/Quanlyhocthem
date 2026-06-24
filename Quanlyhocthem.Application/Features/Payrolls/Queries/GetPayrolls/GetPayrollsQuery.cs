using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrolls
{
    // Query để Admin xem danh sách bảng lương
    public class GetPayrollsQuery : IRequest<Result<List<PayrollDto>>>
    {
        // Lọc theo giảng viên, có thể null
        public Guid? TeacherId { get; set; }

        // Lọc theo tháng, có thể null
        public int? Month { get; set; }

        // Lọc theo năm, có thể null
        public int? Year { get; set; }

        // Lọc theo trạng thái, có thể null
        public PayrollStatus? Status { get; set; }
    }
}