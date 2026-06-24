using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourses
{
    // Query lấy danh sách lớp học của giảng viên đang đăng nhập
    public class GetTeacherCoursesQuery : IRequest<Result<List<TeacherCourseDto>>>
    {
        // Id User của giảng viên lấy từ JWT token
        public Guid TeacherUserId { get; set; }
    }
}