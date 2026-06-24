using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.MarkTuitionInvoiceAsPaid
{
    // Handler xử lý Admin xác nhận hóa đơn đã thanh toán
    public class MarkTuitionInvoiceAsPaidCommandHandler : IRequestHandler<MarkTuitionInvoiceAsPaidCommand, Result>
    {
        private readonly IAppDbContext _context;

        public MarkTuitionInvoiceAsPaidCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(MarkTuitionInvoiceAsPaidCommand request, CancellationToken cancellationToken)
        {
            if (request.TuitionInvoiceId == Guid.Empty)
                return Result.Failure("Id hóa đơn học phí không hợp lệ.");

            var invoice = await _context.TuitionInvoices
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == request.TuitionInvoiceId, cancellationToken);

            if (invoice == null)
                return Result.Failure("Không tìm thấy hóa đơn học phí.");

            if (invoice.Status == TuitionInvoiceStatus.Paid)
                return Result.Failure("Hóa đơn này đã được thanh toán trước đó.");

            try
            {
                invoice.MarkAsPaid(request.PaidAt ?? DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            var notification = new Notification(
                invoice.Student.UserId,
                "Hóa đơn học phí đã thanh toán",
                $"Hóa đơn học phí lớp \"{invoice.Course.Name}\" đã được xác nhận thanh toán.",
                NotificationType.Tuition,
                invoice.Id,
                "TuitionInvoice"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}