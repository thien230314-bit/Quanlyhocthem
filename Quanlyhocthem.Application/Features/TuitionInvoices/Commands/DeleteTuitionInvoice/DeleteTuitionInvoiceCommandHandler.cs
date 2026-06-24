using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.DeleteTuitionInvoice
{
    // Handler xử lý Admin xóa hóa đơn học phí
    public class DeleteTuitionInvoiceCommandHandler : IRequestHandler<DeleteTuitionInvoiceCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteTuitionInvoiceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteTuitionInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request.TuitionInvoiceId == Guid.Empty)
                return Result.Failure("Id hóa đơn học phí không hợp lệ.");

            var invoice = await _context.TuitionInvoices
                .FirstOrDefaultAsync(x => x.Id == request.TuitionInvoiceId, cancellationToken);

            if (invoice == null)
                return Result.Failure("Không tìm thấy hóa đơn học phí.");

            if (invoice.Status == TuitionInvoiceStatus.Paid)
                return Result.Failure("Không thể xóa hóa đơn đã thanh toán.");

            // Xóa thông báo liên quan đến hóa đơn trước
            var notifications = await _context.Notifications
                .Where(x =>
                    x.RelatedEntityId == request.TuitionInvoiceId &&
                    x.RelatedEntityType == "TuitionInvoice")
                .ToListAsync(cancellationToken);

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
            }

            _context.TuitionInvoices.Remove(invoice);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}