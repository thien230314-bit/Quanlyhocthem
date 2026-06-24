using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetPayrolls
{
    // Handler lấy danh sách bảng lương cho Admin
    public class GetPayrollsQueryHandler : IRequestHandler<GetPayrollsQuery, Result<List<PayrollDto>>>
    {
        private readonly IAppDbContext _context;

        public GetPayrollsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<PayrollDto>>> Handle(GetPayrollsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Payrolls
                .Include(x => x.Teacher)
                .AsQueryable();

            if (request.TeacherId.HasValue)
            {
                query = query.Where(x => x.TeacherId == request.TeacherId.Value);
            }

            if (request.Month.HasValue)
            {
                query = query.Where(x => x.Month == request.Month.Value);
            }

            if (request.Year.HasValue)
            {
                query = query.Where(x => x.Year == request.Year.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var payrolls = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenBy(x => x.Teacher.FullName)
                .Select(x => new PayrollDto
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
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<PayrollDto>>.Success(payrolls);
        }
    }
}