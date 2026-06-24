using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseSchedules
{
    // Query để Admin xem danh sách lịch học
    public class GetCourseSchedulesQuery : IRequest<Result<List<CourseScheduleDto>>>
    {
        // Lọc theo lớp học
        public Guid? CourseId { get; set; }

        // Lọc theo giảng viên
        public Guid? TeacherId { get; set; }

        // Lọc theo phòng học
        public Guid? ClassroomId { get; set; }

        // Lọc theo thứ
        public DayOfWeek? DayOfWeek { get; set; }

        // Lọc theo trạng thái hoạt động
        public bool? IsActive { get; set; }
    }
}