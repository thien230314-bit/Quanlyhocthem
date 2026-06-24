using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourses
{
    // Query lấy danh sách lớp học
    public class GetCoursesQuery : IRequest<Result<List<CourseDto>>>
    {
    }
}