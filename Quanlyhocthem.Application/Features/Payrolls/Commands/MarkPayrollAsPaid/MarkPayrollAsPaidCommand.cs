using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.MarkPayrollAsPaid
{
    // Command để Admin đánh dấu bảng lương đã thanh toán
    public class MarkPayrollAsPaidCommand : IRequest<Result>
    {
        // Id bảng lương
        public Guid PayrollId { get; set; }

        // Ngày thanh toán, nếu null thì lấy thời gian hiện tại
        public DateTime? PaidAt { get; set; }
    }
}