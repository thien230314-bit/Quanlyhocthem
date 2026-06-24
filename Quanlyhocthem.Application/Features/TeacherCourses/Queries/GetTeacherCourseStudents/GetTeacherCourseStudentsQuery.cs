using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.TeacherCourses.Queries.GetTeacherCourseStudents
{
    // Query lấy danh sách học sinh trong một lớp của giảng viên
    public class GetTeacherCourseStudentsQuery : IRequest<Result<List<TeacherCourseStudentDto>>>
    {
        // Id User của giảng viên lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id lớp học cần xem danh sách học sinh
        public Guid CourseId { get; set; }
    }
}