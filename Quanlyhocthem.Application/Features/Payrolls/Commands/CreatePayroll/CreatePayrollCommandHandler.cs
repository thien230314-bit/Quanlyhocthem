using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Payrolls.Commands.CreatePayroll
{
    // Handler xử lý Admin tạo bảng lương
    public class CreatePayrollCommandHandler : IRequestHandler<CreatePayrollCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreatePayrollCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
        {
            if (request.TeacherId == Guid.Empty)
                return Result<Guid>.Failure("Id giảng viên không hợp lệ.");

            if (request.Month < 1 || request.Month > 12)
                return Result<Guid>.Failure("Tháng tính lương không hợp lệ.");

            if (request.Year < 2000)
                return Result<Guid>.Failure("Năm tính lương không hợp lệ.");

            if (request.TeachingSessions < 0)
                return Result<Guid>.Failure("Số buổi dạy không được âm.");

            if (request.BaseSalary < 0)
                return Result<Guid>.Failure("Lương cơ bản không được âm.");

            if (request.TeachingSessionAmount < 0)
                return Result<Guid>.Failure("Tiền mỗi buổi dạy không được âm.");

            if (request.BonusAmount < 0)
                return Result<Guid>.Failure("Tiền thưởng không được âm.");

            if (request.DeductionAmount < 0)
                return Result<Guid>.Failure("Tiền trừ không được âm.");

            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherId, cancellationToken);

            if (teacher == null)
                return Result<Guid>.Failure("Không tìm thấy giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<Guid>.Failure("Tài khoản được chọn không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<Guid>.Failure("Không thể tạo lương cho giảng viên đang bị khóa.");

            var exists = await _context.Payrolls
                .AnyAsync(x =>
                    x.TeacherId == request.TeacherId &&
                    x.Month == request.Month &&
                    x.Year == request.Year,
                    cancellationToken);

            if (exists)
                return Result<Guid>.Failure("Giảng viên đã có bảng lương trong tháng/năm này.");

            var payroll = new Payroll(
                request.TeacherId,
                request.Month,
                request.Year,
                request.TeachingSessions,
                request.BaseSalary,
                request.TeachingSessionAmount,
                request.BonusAmount,
                request.DeductionAmount,
                request.Note
            );

            _context.Payrolls.Add(payroll);

            var notification = new Notification(
                request.TeacherId,
                "Có bảng lương mới",
                $"Admin đã tạo bảng lương tháng {request.Month}/{request.Year} cho bạn.",
                NotificationType.General,
                payroll.Id,
                "Payroll"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(payroll.Id);
        }
    }
}