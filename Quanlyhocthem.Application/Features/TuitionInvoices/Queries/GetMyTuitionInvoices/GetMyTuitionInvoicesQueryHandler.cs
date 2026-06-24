using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Queries.GetMyTuitionInvoices
{
    // Handler xử lý học viên xem học phí của mình
    public class GetMyTuitionInvoicesQueryHandler : IRequestHandler<GetMyTuitionInvoicesQuery, Result<List<MyTuitionInvoiceDto>>>
    {
        private readonly IAppDbContext _context;

        public GetMyTuitionInvoicesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<MyTuitionInvoiceDto>>> Handle(GetMyTuitionInvoicesQuery request, CancellationToken cancellationToken)
        {
            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<List<MyTuitionInvoiceDto>>.Failure("Không tìm thấy hồ sơ học viên.");

            var query = _context.TuitionInvoices
                .Include(x => x.Course)
                .Where(x => x.StudentId == student.Id);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var now = DateTime.UtcNow;

            var invoices = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new MyTuitionInvoiceDto
                {
                    TuitionInvoiceId = x.Id,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    Status = x.Status,
                    IsOverdue = x.Status == TuitionInvoiceStatus.Unpaid && x.DueDate < now,
                    PaidAt = x.PaidAt,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<MyTuitionInvoiceDto>>.Success(invoices);
        }
    }
}