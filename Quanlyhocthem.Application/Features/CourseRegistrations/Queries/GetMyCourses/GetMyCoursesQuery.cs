using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Queries.GetMyCourses
{
    // Query lấy danh sách khóa học học viên đã đăng ký
    public class GetMyCoursesQuery : IRequest<Result<List<MyCourseDto>>>
    {
        // UserId của học viên hiện tại
        public Guid StudentUserId { get; set; }
    }
}