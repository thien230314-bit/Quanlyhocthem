using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetAvailableCourses
{
    // Query lấy danh sách khóa học học viên có thể đăng ký
    public class GetAvailableCoursesQuery : IRequest<Result<List<AvailableCourseDto>>>
    {
        // UserId của học viên hiện tại
        public Guid StudentUserId { get; set; }
    }
}