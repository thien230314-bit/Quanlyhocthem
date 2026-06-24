using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoiceById
{
    // Handler lấy chi tiết hóa đơn học phí
    public class GetTuitionInvoiceByIdQueryHandler : IRequestHandler<GetTuitionInvoiceByIdQuery, Result<TuitionInvoiceDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetTuitionInvoiceByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<TuitionInvoiceDetailDto>> Handle(GetTuitionInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TuitionInvoiceId == Guid.Empty)
                return Result<TuitionInvoiceDetailDto>.Failure("Id hóa đơn học phí không hợp lệ.");

            var invoice = await _context.TuitionInvoices
                .Where(x => x.Id == request.TuitionInvoiceId)
                .Select(x => new TuitionInvoiceDetailDto
                {
                    TuitionInvoiceId = x.Id,
                    StudentId = x.StudentId,
                    StudentCode = x.Student.StudentCode,
                    StudentName = x.Student.User.FullName,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    Status = x.Status,
                    PaidAt = x.PaidAt,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice == null)
                return Result<TuitionInvoiceDetailDto>.Failure("Không tìm thấy hóa đơn học phí.");

            return Result<TuitionInvoiceDetailDto>.Success(invoice);
        }
    }
}