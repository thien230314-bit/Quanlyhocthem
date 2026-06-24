using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.UpdatePayroll
{
    // Handler xử lý Admin cập nhật bảng lương
    public class UpdatePayrollCommandHandler : IRequestHandler<UpdatePayrollCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdatePayrollCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdatePayrollCommand request, CancellationToken cancellationToken)
        {
            if (request.PayrollId == Guid.Empty)
                return Result.Failure("Id bảng lương không hợp lệ.");

            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(x => x.Id == request.PayrollId, cancellationToken);

            if (payroll == null)
                return Result.Failure("Không tìm thấy bảng lương.");

            try
            {
                payroll.Update(
                    request.TeachingSessions,
                    request.BaseSalary,
                    request.TeachingSessionAmount,
                    request.BonusAmount,
                    request.DeductionAmount,
                    request.Note
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}