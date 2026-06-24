using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetStudentWeeklySchedule
{
    // Query để học viên xem thời khóa biểu tuần của mình
    public class GetStudentWeeklyScheduleQuery : IRequest<Result<List<StudentWeeklyScheduleDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Ngày bất kỳ trong tuần cần xem, nếu null thì lấy tuần hiện tại
        public DateTime? WeekDate { get; set; }
    }
}