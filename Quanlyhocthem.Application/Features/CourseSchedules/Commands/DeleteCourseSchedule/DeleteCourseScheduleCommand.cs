using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.DeleteCourseSchedule
{
    // Command để Admin xóa lịch học
    public class DeleteCourseScheduleCommand : IRequest<Result>
    {
        // Id lịch học cần xóa
        public Guid CourseScheduleId { get; set; }
    }
}