using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Commands.DeleteCourse
{
    // Handler xử lý xóa lớp học
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result.Failure("Id lớp học không hợp lệ.");

            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result.Failure("Không tìm thấy lớp học.");

            // Nếu lớp đã có học viên đăng ký thì không cho xóa
            var hasEnrollments = await _context.Enrollments
                .AnyAsync(x => x.CourseId == request.CourseId, cancellationToken);

            if (hasEnrollments)
                return Result.Failure("Không thể xóa lớp học vì đã có học viên đăng ký.");

            // Nếu lớp đã có bài tập thì không cho xóa
            var hasAssignments = await _context.Assignments
                .AnyAsync(x => x.CourseId == request.CourseId, cancellationToken);

            if (hasAssignments)
                return Result.Failure("Không thể xóa lớp học vì đã có bài tập.");

            // Nếu lớp đã có ca điểm danh thì không cho xóa
            var hasAttendanceSessions = await _context.AttendanceSessions
                .AnyAsync(x => x.CourseId == request.CourseId, cancellationToken);

            if (hasAttendanceSessions)
                return Result.Failure("Không thể xóa lớp học vì đã có dữ liệu điểm danh.");

            // Nếu lớp đã có hóa đơn học phí thì không cho xóa
            var hasTuitionInvoices = await _context.TuitionInvoices
                .AnyAsync(x => x.CourseId == request.CourseId, cancellationToken);

            if (hasTuitionInvoices)
                return Result.Failure("Không thể xóa lớp học vì đã có hóa đơn học phí.");

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}