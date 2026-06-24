using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Commands.RegisterCourse
{
    // Handler xử lý học viên đăng ký lớp học
    public class RegisterCourseCommandHandler : IRequestHandler<RegisterCourseCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public RegisterCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(RegisterCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result<Guid>.Failure("Id lớp học không hợp lệ.");

            // Tìm hồ sơ học viên theo UserId trong token
            var student = await _context.Students
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == request.StudentUserId, cancellationToken);

            if (student == null)
                return Result<Guid>.Failure("Không tìm thấy hồ sơ học viên.");

            // Kiểm tra tài khoản học viên còn hoạt động không
            if (!student.User.IsActive)
                return Result<Guid>.Failure("Tài khoản học viên đang bị khóa.");

            // Tìm lớp học
            var course = await _context.Courses
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<Guid>.Failure("Không tìm thấy lớp học.");

            if (!course.IsActive)
                return Result<Guid>.Failure("Lớp học này đã ngưng hoạt động.");

            // Kiểm tra học viên đã đăng ký lớp này chưa
            var alreadyRegistered = await _context.Enrollments
                .AnyAsync(x =>
                    x.StudentId == student.Id &&
                    x.CourseId == course.Id &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (alreadyRegistered)
                return Result<Guid>.Failure("Bạn đã đăng ký lớp học này rồi.");

            // Kiểm tra sĩ số lớp
            var currentStudents = await _context.Enrollments
                .CountAsync(x =>
                    x.CourseId == course.Id &&
                    x.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (currentStudents >= course.MaxStudents)
                return Result<Guid>.Failure("Lớp học đã đủ sĩ số.");

            // Tạo đăng ký lớp học
            var enrollment = new Enrollment(course.Id, student.Id);

            _context.Enrollments.Add(enrollment);

            // Nếu lớp có học phí thì tự tạo hóa đơn học phí cho học viên
            if (course.TuitionFee > 0)
            {
                var invoiceExists = await _context.TuitionInvoices
                    .AnyAsync(x =>
                        x.StudentId == student.Id &&
                        x.CourseId == course.Id &&
                        x.Status != TuitionInvoiceStatus.Cancelled,
                        cancellationToken);

                if (!invoiceExists)
                {
                    var dueDate = DateTime.UtcNow.AddDays(30);

                    var tuitionInvoice = new TuitionInvoice(
                        student.Id,
                        course.Id,
                        course.TuitionFee,
                        dueDate,
                        $"Học phí lớp {course.Name}"
                    );

                    _context.TuitionInvoices.Add(tuitionInvoice);

                    var notification = new Notification(
                        student.UserId,
                        "Có hóa đơn học phí mới",
                        $"Bạn có hóa đơn học phí lớp \"{course.Name}\" với số tiền {course.TuitionFee}.",
                        NotificationType.Tuition,
                        tuitionInvoice.Id,
                        "TuitionInvoice"
                    );

                    _context.Notifications.Add(notification);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(enrollment.Id);
        }
    }
}