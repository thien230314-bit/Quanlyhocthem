using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetTuitionInvoices
{
    // Handler lấy danh sách hóa đơn học phí cho Admin
    public class GetTuitionInvoicesQueryHandler : IRequestHandler<GetTuitionInvoicesQuery, Result<List<TuitionInvoiceDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTuitionInvoicesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TuitionInvoiceDto>>> Handle(GetTuitionInvoicesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TuitionInvoices
                .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                .Include(x => x.Course)
                .AsQueryable();

            if (request.StudentId.HasValue)
            {
                query = query.Where(x => x.StudentId == request.StudentId.Value);
            }

            if (request.CourseId.HasValue)
            {
                query = query.Where(x => x.CourseId == request.CourseId.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var invoices = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TuitionInvoiceDto
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
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<TuitionInvoiceDto>>.Success(invoices);
        }
    }
}