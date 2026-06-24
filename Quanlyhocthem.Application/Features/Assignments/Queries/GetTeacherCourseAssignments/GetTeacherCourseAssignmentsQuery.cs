using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetTeacherCourseAssignments
{
    // Query để giảng viên xem danh sách bài tập đã giao trong một lớp
    public class GetTeacherCourseAssignmentsQuery : IRequest<Result<List<TeacherCourseAssignmentDto>>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id lớp học cần xem bài tập
        public Guid CourseId { get; set; }
    }
}