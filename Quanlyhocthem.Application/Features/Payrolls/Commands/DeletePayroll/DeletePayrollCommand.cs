using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.DeletePayroll
{
    // Command để Admin xóa bảng lương
    public class DeletePayrollCommand : IRequest<Result>
    {
        // Id bảng lương cần xóa
        public Guid PayrollId { get; set; }
    }
}