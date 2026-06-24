using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.ConfirmPayroll
{
    // Command để Admin xác nhận bảng lương
    public class ConfirmPayrollCommand : IRequest<Result>
    {
        // Id bảng lương
        public Guid PayrollId { get; set; }
    }
}