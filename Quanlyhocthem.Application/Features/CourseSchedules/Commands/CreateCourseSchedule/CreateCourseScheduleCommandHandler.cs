using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.CreateCourseSchedule
{
    // Handler xử lý Admin tạo lịch học cho lớp
    public class CreateCourseScheduleCommandHandler : IRequestHandler<CreateCourseScheduleCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateCourseScheduleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateCourseScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId == Guid.Empty)
                return Result<Guid>.Failure("Id lớp học không hợp lệ.");

            if (request.TeacherId == Guid.Empty)
                return Result<Guid>.Failure("Id giảng viên không hợp lệ.");

            if (request.ClassroomId == Guid.Empty)
                return Result<Guid>.Failure("Id phòng học không hợp lệ.");

            if (request.StartTime >= request.EndTime)
                return Result<Guid>.Failure("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            if (request.EffectiveTo.HasValue && request.EffectiveTo.Value.Date < request.EffectiveFrom.Date)
                return Result<Guid>.Failure("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            var effectiveFrom = request.EffectiveFrom.Date;
            var effectiveTo = request.EffectiveTo?.Date;

            // Kiểm tra lớp học tồn tại
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

            if (course == null)
                return Result<Guid>.Failure("Không tìm thấy lớp học.");

            if (!course.IsActive)
                return Result<Guid>.Failure("Lớp học đã ngưng hoạt động.");

            if (!course.TeacherId.HasValue)
                return Result<Guid>.Failure("Lớp học chưa được gán giảng viên.");

            if (course.TeacherId.Value != request.TeacherId)
                return Result<Guid>.Failure("Giảng viên được chọn không phải giảng viên phụ trách lớp này.");

            // Kiểm tra giảng viên
            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherId, cancellationToken);

            if (teacher == null)
                return Result<Guid>.Failure("Không tìm thấy giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result<Guid>.Failure("Tài khoản được chọn không phải giảng viên.");

            if (!teacher.IsActive)
                return Result<Guid>.Failure("Tài khoản giảng viên đang bị khóa.");

            // Kiểm tra phòng học
            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(x => x.Id == request.ClassroomId, cancellationToken);

            if (classroom == null)
                return Result<Guid>.Failure("Không tìm thấy phòng học.");

            if (!classroom.IsActive)
                return Result<Guid>.Failure("Phòng học đã ngưng hoạt động.");

            // Kiểm tra trùng lịch dạy của giảng viên
            var teacherConflict = await _context.CourseSchedules
                .AnyAsync(x =>
                    x.IsActive &&
                    x.TeacherId == request.TeacherId &&
                    x.DayOfWeek == request.DayOfWeek &&
                    x.StartTime < request.EndTime &&
                    request.StartTime < x.EndTime &&
                    effectiveFrom <= (x.EffectiveTo ?? DateTime.MaxValue) &&
                    (effectiveTo ?? DateTime.MaxValue) >= x.EffectiveFrom,
                    cancellationToken);

            if (teacherConflict)
                return Result<Guid>.Failure("Giảng viên đã có lịch dạy trùng thời gian.");

            // Kiểm tra trùng lịch phòng học
            var classroomConflict = await _context.CourseSchedules
                .AnyAsync(x =>
                    x.IsActive &&
                    x.ClassroomId == request.ClassroomId &&
                    x.DayOfWeek == request.DayOfWeek &&
                    x.StartTime < request.EndTime &&
                    request.StartTime < x.EndTime &&
                    effectiveFrom <= (x.EffectiveTo ?? DateTime.MaxValue) &&
                    (effectiveTo ?? DateTime.MaxValue) >= x.EffectiveFrom,
                    cancellationToken);

            if (classroomConflict)
                return Result<Guid>.Failure("Phòng học đã có lịch trùng thời gian.");

            var schedule = new CourseSchedule(
                request.CourseId,
                request.TeacherId,
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.Note
            );

            _context.CourseSchedules.Add(schedule);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(schedule.Id);
        }
    }
}