using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule
{
    // Command để Admin cập nhật lịch học
    public class UpdateCourseScheduleCommand : IRequest<Result>
    {
        // Id lịch học cần cập nhật
        public Guid CourseScheduleId { get; set; }

        // Id giảng viên mới
        public Guid TeacherId { get; set; }

        // Id phòng học mới
        public Guid ClassroomId { get; set; }

        // Thứ trong tuần
        public DayOfWeek DayOfWeek { get; set; }

        // Giờ bắt đầu
        public TimeSpan StartTime { get; set; }

        // Giờ kết thúc
        public TimeSpan EndTime { get; set; }

        // Ngày bắt đầu áp dụng
        public DateTime EffectiveFrom { get; set; }

        // Ngày kết thúc áp dụng
        public DateTime? EffectiveTo { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}