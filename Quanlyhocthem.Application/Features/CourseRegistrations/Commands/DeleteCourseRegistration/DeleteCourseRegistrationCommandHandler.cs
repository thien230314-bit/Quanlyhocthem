using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Commands.DeleteCourseRegistration
{
    // Handler xử lý học viên hủy đăng ký lớp học
    public class DeleteCourseRegistrationCommandHandler : IRequestHandler<DeleteCourseRegistrationCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteCourseRegistrationCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCourseRegistrationCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result.Failure("Id lớp học không hợp lệ.");

            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result.Failure("Không tìm thấy hồ sơ học viên.");

            // Tìm đăng ký lớp đang hoạt động
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == request.CourseId &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (enrollment == null)
                return Result.Failure("Không tìm thấy đăng ký lớp học cần hủy.");

            // Nếu đã có dữ liệu điểm danh thì không cho xóa đăng ký
            var hasAttendance = await _context.Attendances
                .AnyAsync(x => x.EnrollmentId == enrollment.Id, cancellationToken);

            if (hasAttendance)
                return Result.Failure("Không thể hủy đăng ký vì học viên đã có dữ liệu điểm danh trong lớp này.");

            // Nếu đã có bài nộp thì không cho xóa đăng ký
            var hasSubmission = await _context.Submissions
                .AnyAsync(x => x.EnrollmentId == enrollment.Id, cancellationToken);

            if (hasSubmission)
                return Result.Failure("Không thể hủy đăng ký vì học viên đã có bài nộp trong lớp này.");

            // Nếu đã có hóa đơn học phí thì không cho xóa đăng ký
            var hasTuitionInvoice = await _context.TuitionInvoices
                .AnyAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == request.CourseId,
                    cancellationToken);

            if (hasTuitionInvoice)
                return Result.Failure("Không thể hủy đăng ký vì học viên đã có hóa đơn học phí trong lớp này.");

            _context.Enrollments.Remove(enrollment);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}