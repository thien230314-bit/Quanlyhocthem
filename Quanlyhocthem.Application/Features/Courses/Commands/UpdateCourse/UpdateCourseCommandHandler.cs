using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Courses.Commands.UpdateCourse
{
    // Handler xử lý cập nhật lớp học
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result.Failure("Id lớp học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result.Failure("Tên lớp học không được để trống.");

            if (request.TuitionFee < 0)
                return Result.Failure("Học phí không được âm.");

            if (request.MaxStudents < 1)
                return Result.Failure("Sĩ số tối đa phải lớn hơn 0.");

            if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
                return Result.Failure("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            var course = await _context.Courses
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result.Failure("Không tìm thấy lớp học.");

            // Không cho giảm sĩ số tối đa nhỏ hơn số học viên đang học
            var currentStudents = course.Enrollments.Count(x => x.Status == EnrollmentStatus.Active);

            if (request.MaxStudents < currentStudents)
                return Result.Failure($"Sĩ số tối đa không được nhỏ hơn số học viên đang học hiện tại là {currentStudents}.");

            // Kiểm tra môn học
            var subjectExists = await _context.Subjects
                .AnyAsync(x => x.Id == request.SubjectId && x.IsActive, cancellationToken);

            if (!subjectExists)
                return Result.Failure("Môn học không tồn tại hoặc đã ngưng hoạt động.");

            // Kiểm tra khóa học viên
            var batchExists = await _context.Batches
                .AnyAsync(x => x.Id == request.BatchId && x.IsActive, cancellationToken);

            if (!batchExists)
                return Result.Failure("Khóa học viên không tồn tại hoặc đã ngưng hoạt động.");

            // Kiểm tra phòng học nếu có chọn
            if (request.ClassroomId.HasValue)
            {
                var classroomExists = await _context.Classrooms
                    .AnyAsync(x => x.Id == request.ClassroomId.Value && x.IsActive, cancellationToken);

                if (!classroomExists)
                    return Result.Failure("Phòng học không tồn tại hoặc đã ngưng hoạt động.");
            }

            // Kiểm tra giảng viên nếu có chọn
            if (request.TeacherId.HasValue)
            {
                var teacher = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == request.TeacherId.Value, cancellationToken);

                if (teacher == null)
                    return Result.Failure("Giảng viên không tồn tại.");

                if (teacher.Role != Role.Teacher)
                    return Result.Failure("Người được gán vào lớp học phải có vai trò Giảng viên.");

                if (!teacher.IsActive)
                    return Result.Failure("Không thể gán lớp học cho giảng viên đang bị khóa.");
            }

            course.UpdateDetails(
                request.Name,
                request.Description,
                request.TuitionFee,
                request.MaxStudents,
                request.StartDate,
                request.EndDate
            );

            course.UpdateRelations(
                request.SubjectId,
                request.BatchId,
                request.ClassroomId,
                request.TeacherId
            );

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}