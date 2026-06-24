using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrollById
{
    // Handler lấy chi tiết bảng lương
    public class GetPayrollByIdQueryHandler : IRequestHandler<GetPayrollByIdQuery, Result<PayrollDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetPayrollByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PayrollDetailDto>> Handle(GetPayrollByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.PayrollId == Guid.Empty)
                return Result<PayrollDetailDto>.Failure("Id bảng lương không hợp lệ.");

            var payroll = await _context.Payrolls
                .Where(x => x.Id == request.PayrollId)
                .Select(x => new PayrollDetailDto
                {
                    PayrollId = x.Id,
                    TeacherId = x.TeacherId,
                    TeacherName = x.Teacher.FullName,
                    TeacherEmail = x.Teacher.Email,
                    Month = x.Month,
                    Year = x.Year,
                    TeachingSessions = x.TeachingSessions,
                    BaseSalary = x.BaseSalary,
                    TeachingSessionAmount = x.TeachingSessionAmount,
                    BonusAmount = x.BonusAmount,
                    DeductionAmount = x.DeductionAmount,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status,
                    PaidAt = x.PaidAt,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (payroll == null)
                return Result<PayrollDetailDto>.Failure("Không tìm thấy bảng lương.");

            return Result<PayrollDetailDto>.Success(payroll);
        }
    }
}