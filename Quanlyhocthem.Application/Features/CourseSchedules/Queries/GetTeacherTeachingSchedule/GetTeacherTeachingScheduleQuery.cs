using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetTeacherTeachingSchedule
{
    // Query để giảng viên xem lịch dạy tuần của mình
    public class GetTeacherTeachingScheduleQuery : IRequest<Result<List<TeacherTeachingScheduleDto>>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Ngày bất kỳ trong tuần cần xem, nếu null thì lấy tuần hiện tại
        public DateTime? WeekDate { get; set; }

        // Lọc theo lớp học, có thể null
        public Guid? CourseId { get; set; }
    }
}