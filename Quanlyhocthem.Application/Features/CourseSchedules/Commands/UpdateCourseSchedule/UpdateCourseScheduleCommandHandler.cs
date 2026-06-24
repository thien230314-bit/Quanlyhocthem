using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule
{
    // Handler xử lý Admin cập nhật lịch học
    public class UpdateCourseScheduleCommandHandler : IRequestHandler<UpdateCourseScheduleCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateCourseScheduleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateCourseScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseScheduleId == Guid.Empty)
                return Result.Failure("Id lịch học không hợp lệ.");

            if (request.TeacherId == Guid.Empty)
                return Result.Failure("Id giảng viên không hợp lệ.");

            if (request.ClassroomId == Guid.Empty)
                return Result.Failure("Id phòng học không hợp lệ.");

            if (request.StartTime >= request.EndTime)
                return Result.Failure("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            if (request.EffectiveTo.HasValue && request.EffectiveTo.Value.Date < request.EffectiveFrom.Date)
                return Result.Failure("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            var effectiveFrom = request.EffectiveFrom.Date;
            var effectiveTo = request.EffectiveTo?.Date;

            var schedule = await _context.CourseSchedules
                .FirstOrDefaultAsync(x => x.Id == request.CourseScheduleId, cancellationToken);

            if (schedule == null)
                return Result.Failure("Không tìm thấy lịch học.");

            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == schedule.CourseId, cancellationToken);

            if (course == null)
                return Result.Failure("Không tìm thấy lớp học của lịch này.");

            if (!course.IsActive)
                return Result.Failure("Lớp học đã ngưng hoạt động.");

            if (!course.TeacherId.HasValue)
                return Result.Failure("Lớp học chưa được gán giảng viên.");

            if (course.TeacherId.Value != request.TeacherId)
                return Result.Failure("Giảng viên được chọn không phải giảng viên phụ trách lớp này.");

            var teacher = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.TeacherId, cancellationToken);

            if (teacher == null)
                return Result.Failure("Không tìm thấy giảng viên.");

            if (teacher.Role != Role.Teacher)
                return Result.Failure("Tài khoản được chọn không phải giảng viên.");

            if (!teacher.IsActive)
                return Result.Failure("Tài khoản giảng viên đang bị khóa.");

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(x => x.Id == request.ClassroomId, cancellationToken);

            if (classroom == null)
                return Result.Failure("Không tìm thấy phòng học.");

            if (!classroom.IsActive)
                return Result.Failure("Phòng học đã ngưng hoạt động.");

            // Kiểm tra trùng lịch dạy của giảng viên, bỏ qua chính lịch đang sửa
            var teacherConflict = await _context.CourseSchedules
                .AnyAsync(x =>
                    x.Id != request.CourseScheduleId &&
                    x.IsActive &&
                    x.TeacherId == request.TeacherId &&
                    x.DayOfWeek == request.DayOfWeek &&
                    x.StartTime < request.EndTime &&
                    request.StartTime < x.EndTime &&
                    effectiveFrom <= (x.EffectiveTo ?? DateTime.MaxValue) &&
                    (effectiveTo ?? DateTime.MaxValue) >= x.EffectiveFrom,
                    cancellationToken);

            if (teacherConflict)
                return Result.Failure("Giảng viên đã có lịch dạy trùng thời gian.");

            // Kiểm tra trùng lịch phòng học, bỏ qua chính lịch đang sửa
            var classroomConflict = await _context.CourseSchedules
                .AnyAsync(x =>
                    x.Id != request.CourseScheduleId &&
                    x.IsActive &&
                    x.ClassroomId == request.ClassroomId &&
                    x.DayOfWeek == request.DayOfWeek &&
                    x.StartTime < request.EndTime &&
                    request.StartTime < x.EndTime &&
                    effectiveFrom <= (x.EffectiveTo ?? DateTime.MaxValue) &&
                    (effectiveTo ?? DateTime.MaxValue) >= x.EffectiveFrom,
                    cancellationToken);

            if (classroomConflict)
                return Result.Failure("Phòng học đã có lịch trùng thời gian.");

            try
            {
                schedule.Update(
                    request.TeacherId,
                    request.ClassroomId,
                    request.DayOfWeek,
                    request.StartTime,
                    request.EndTime,
                    request.EffectiveFrom,
                    request.EffectiveTo,
                    request.Note
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}