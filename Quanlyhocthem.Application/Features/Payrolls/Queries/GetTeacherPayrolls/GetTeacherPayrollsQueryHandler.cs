using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrolls
{
    // Handler lấy danh sách bảng lương của giảng viên
    public class GetTeacherPayrollsQueryHandler : IRequestHandler<GetTeacherPayrollsQuery, Result<List<TeacherPayrollDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherPayrollsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherPayrollDto>>> Handle(GetTeacherPayrollsQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<List<TeacherPayrollDto>>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<List<TeacherPayrollDto>>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<List<TeacherPayrollDto>>.Failure("Tài khoản giảng viên đang bị khóa.");

            var query = _context.Payrolls
                .Where(x => x.TeacherId == request.TeacherUserId);

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
                .Select(x => new TeacherPayrollDto
                {
                    PayrollId = x.Id,
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
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherPayrollDto>>.Success(payrolls);
        }
    }
}