using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrollById
{
    // Query lấy chi tiết một bảng lương
    public class GetPayrollByIdQuery : IRequest<Result<PayrollDetailDto>>
    {
        // Id bảng lương
        public Guid PayrollId { get; set; }
    }
}