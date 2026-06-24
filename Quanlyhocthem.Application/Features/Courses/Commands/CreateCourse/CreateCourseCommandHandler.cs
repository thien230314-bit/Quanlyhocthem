using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Courses.Commands.CreateCourse
{
    // Handler xử lý tạo lớp học cụ thể
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra tên lớp học
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<Guid>.Failure("Tên lớp học không được để trống.");

            // Kiểm tra học phí
            if (request.TuitionFee < 0)
                return Result<Guid>.Failure("Học phí không được âm.");

            // Kiểm tra sĩ số
            if (request.MaxStudents < 1)
                return Result<Guid>.Failure("Sĩ số tối đa phải lớn hơn 0.");

            // Kiểm tra ngày học
            if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
                return Result<Guid>.Failure("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            // Kiểm tra môn học có tồn tại không
            var subjectExists = await _context.Subjects
                .AnyAsync(x => x.Id == request.SubjectId, cancellationToken);

            if (!subjectExists)
                return Result<Guid>.Failure("Môn học không tồn tại.");

            // Kiểm tra khóa học viên có tồn tại không
            var batchExists = await _context.Batches
                .AnyAsync(x => x.Id == request.BatchId, cancellationToken);

            if (!batchExists)
                return Result<Guid>.Failure("Khóa học viên không tồn tại.");

            // Nếu có chọn phòng học thì kiểm tra phòng có tồn tại không
            if (request.ClassroomId.HasValue)
            {
                var classroomExists = await _context.Classrooms
                    .AnyAsync(x => x.Id == request.ClassroomId.Value, cancellationToken);

                if (!classroomExists)
                    return Result<Guid>.Failure("Phòng học không tồn tại.");
            }

            // Nếu có chọn giảng viên thì kiểm tra user đó có đúng role Teacher không
            if (request.TeacherId.HasValue)
            {
                var teacher = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == request.TeacherId.Value, cancellationToken);

                if (teacher == null)
                    return Result<Guid>.Failure("Giảng viên không tồn tại.");

                if (teacher.Role != Role.Teacher)
                    return Result<Guid>.Failure("Người được gán vào lớp học phải có vai trò Giảng viên.");

                if (!teacher.IsActive)
                    return Result<Guid>.Failure("Không thể gán lớp học cho giảng viên đang bị khóa.");
            }

            // Tạo lớp học cụ thể
            var course = new Course(
                request.Name,
                request.Description,
                request.TuitionFee,
                request.MaxStudents,
                request.SubjectId,
                request.BatchId,
                request.ClassroomId,
                request.TeacherId,
                request.StartDate,
                request.EndDate
            );

            _context.Courses.Add(course);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(course.Id);
        }
    }
}