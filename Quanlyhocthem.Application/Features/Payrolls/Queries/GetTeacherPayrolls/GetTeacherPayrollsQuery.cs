using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrolls
{
    // Query để giảng viên xem danh sách bảng lương của mình
    public class GetTeacherPayrollsQuery : IRequest<Result<List<TeacherPayrollDto>>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Lọc theo tháng, có thể null
        public int? Month { get; set; }

        // Lọc theo năm, có thể null
        public int? Year { get; set; }

        // Lọc theo trạng thái, có thể null
        public PayrollStatus? Status { get; set; }
    }
}