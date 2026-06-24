using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.ConfirmPayroll
{
    // Handler xử lý Admin xác nhận bảng lương
    public class ConfirmPayrollCommandHandler : IRequestHandler<ConfirmPayrollCommand, Result>
    {
        private readonly IAppDbContext _context;

        public ConfirmPayrollCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ConfirmPayrollCommand request, CancellationToken cancellationToken)
        {
            if (request.PayrollId == Guid.Empty)
                return Result.Failure("Id bảng lương không hợp lệ.");

            var payroll = await _context.Payrolls
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == request.PayrollId, cancellationToken);

            if (payroll == null)
                return Result.Failure("Không tìm thấy bảng lương.");

            try
            {
                payroll.Confirm();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            var notification = new Notification(
                payroll.TeacherId,
                "Bảng lương đã được xác nhận",
                $"Bảng lương tháng {payroll.Month}/{payroll.Year} của bạn đã được Admin xác nhận.",
                NotificationType.General,
                payroll.Id,
                "Payroll"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}