using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.DeletePayroll
{
    // Handler xử lý Admin xóa bảng lương
    public class DeletePayrollCommandHandler : IRequestHandler<DeletePayrollCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeletePayrollCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
        {
            if (request.PayrollId == Guid.Empty)
                return Result.Failure("Id bảng lương không hợp lệ.");

            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(x => x.Id == request.PayrollId, cancellationToken);

            if (payroll == null)
                return Result.Failure("Không tìm thấy bảng lương.");

            if (payroll.Status == PayrollStatus.Paid)
                return Result.Failure("Không thể xóa bảng lương đã thanh toán.");

            var notifications = await _context.Notifications
                .Where(x =>
                    x.RelatedEntityId == request.PayrollId &&
                    x.RelatedEntityType == "Payroll")
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
            }

            _context.Payrolls.Remove(payroll);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}