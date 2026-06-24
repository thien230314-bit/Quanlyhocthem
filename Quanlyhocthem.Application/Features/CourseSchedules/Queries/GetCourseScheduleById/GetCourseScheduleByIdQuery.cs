using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseScheduleById
{
    // Query lấy chi tiết một lịch học
    public class GetCourseScheduleByIdQuery : IRequest<Result<CourseScheduleDetailDto>>
    {
        // Id lịch học
        public Guid CourseScheduleId { get; set; }
    }
}