using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.TuitionInvoices.Commands.CreateTuitionInvoice
{
    // Handler xử lý Admin tạo hóa đơn học phí
    public class CreateTuitionInvoiceCommandHandler : IRequestHandler<CreateTuitionInvoiceCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateTuitionInvoiceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateTuitionInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request.StudentId == Guid.Empty)
                return Result<Guid>.Failure("Id học viên không hợp lệ.");

            if (request.CourseId == Guid.Empty)
                return Result<Guid>.Failure("Id lớp học không hợp lệ.");

            if (request.Amount <= 0)
                return Result<Guid>.Failure("Số tiền học phí phải lớn hơn 0.");

            var student = await _context.Students
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

            if (student == null)
                return Result<Guid>.Failure("Không tìm thấy học viên.");

            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<Guid>.Failure("Không tìm thấy lớp học.");

            var hasEnrollment = await _context.Enrollments
                .AnyAsync(x =>
                    x.StudentId == request.StudentId &&
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (!hasEnrollment)
                return Result<Guid>.Failure("Học viên chưa đăng ký lớp học này.");

            var invoiceExists = await _context.TuitionInvoices
                .AnyAsync(x =>
                    x.StudentId == request.StudentId &&
                    x.CourseId == request.CourseId &&
                    x.Status != TuitionInvoiceStatus.Cancelled,
                    cancellationToken);

            if (invoiceExists)
                return Result<Guid>.Failure("Học viên đã có hóa đơn học phí cho lớp này.");

            var invoice = new TuitionInvoice(
                request.StudentId,
                request.CourseId,
                request.Amount,
                request.DueDate,
                request.Note
            );

            _context.TuitionInvoices.Add(invoice);

            var notification = new Notification(
                student.UserId,
                "Có hóa đơn học phí mới",
                $"Bạn có hóa đơn học phí lớp \"{course.Name}\" với số tiền {request.Amount}.",
                NotificationType.Tuition,
                invoice.Id,
                "TuitionInvoice"
            );

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(invoice.Id);
        }
    }
}