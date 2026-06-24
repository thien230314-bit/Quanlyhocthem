using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetStudentAssignments
{
    // Query để học viên xem bài tập trong các lớp đã đăng ký
    public class GetStudentAssignmentsQuery : IRequest<Result<List<StudentAssignmentDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }
    }
}