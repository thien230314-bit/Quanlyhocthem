using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrollById
{
    // Handler lấy chi tiết bảng lương của giảng viên
    public class GetTeacherPayrollByIdQueryHandler : IRequestHandler<GetTeacherPayrollByIdQuery, Result<TeacherPayrollDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetTeacherPayrollByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<TeacherPayrollDetailDto>> Handle(GetTeacherPayrollByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.PayrollId == Guid.Empty)
                return Result<TeacherPayrollDetailDto>.Failure("Id bảng lương không hợp lệ.");

            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherUserId, cancellationToken);

            if (teacher == null)
                return Result<TeacherPayrollDetailDto>.Failure("Không tìm thấy tài khoản giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<TeacherPayrollDetailDto>.Failure("Tài khoản hiện tại không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<TeacherPayrollDetailDto>.Failure("Tài khoản giảng viên đang bị khóa.");

            var payroll = await _context.Payrolls
                .Where(x =>
                    x.Id == request.PayrollId &&
                    x.TeacherId == request.TeacherUserId)
                .Select(x => new TeacherPayrollDetailDto
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
                .FirstOrDefaultAsync(cancellationToken);

            if (payroll == null)
                return Result<TeacherPayrollDetailDto>.Failure("Không tìm thấy bảng lương của bạn.");

            return Result<TeacherPayrollDetailDto>.Success(payroll);
        }
    }
}