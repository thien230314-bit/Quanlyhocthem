using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.CreateCourseSchedule
{
    // Command để Admin tạo lịch học cho lớp
    public class CreateCourseScheduleCommand : IRequest<Result<Guid>>
    {
        // Id lớp học
        public Guid CourseId { get; set; }

        // Id giảng viên dạy ca này
        public Guid TeacherId { get; set; }

        // Id phòng học
        public Guid ClassroomId { get; set; }

        // Thứ trong tuần: Sunday = 0, Monday = 1, Tuesday = 2...
        public DayOfWeek DayOfWeek { get; set; }

        // Giờ bắt đầu, ví dụ: "18:00:00"
        public TimeSpan StartTime { get; set; }

        // Giờ kết thúc, ví dụ: "20:00:00"
        public TimeSpan EndTime { get; set; }

        // Ngày bắt đầu áp dụng lịch
        public DateTime EffectiveFrom { get; set; }

        // Ngày kết thúc áp dụng lịch, có thể null
        public DateTime? EffectiveTo { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}