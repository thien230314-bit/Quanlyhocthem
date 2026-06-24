using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.UpdateTuitionInvoice
{
    // Handler xử lý Admin cập nhật hóa đơn học phí
    public class UpdateTuitionInvoiceCommandHandler : IRequestHandler<UpdateTuitionInvoiceCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateTuitionInvoiceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateTuitionInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request.TuitionInvoiceId == Guid.Empty)
                return Result.Failure("Id hóa đơn học phí không hợp lệ.");

            if (request.Amount <= 0)
                return Result.Failure("Số tiền học phí phải lớn hơn 0.");

            var invoice = await _context.TuitionInvoices
                .FirstOrDefaultAsync(x => x.Id == request.TuitionInvoiceId, cancellationToken);

            if (invoice == null)
                return Result.Failure("Không tìm thấy hóa đơn học phí.");

            try
            {
                invoice.Update(request.Amount, request.DueDate, request.Note);
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