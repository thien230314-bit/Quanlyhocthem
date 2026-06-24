using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Submissions.Queries.GetAssignmentSubmissions
{
    // Query để giảng viên xem danh sách bài nộp của một bài tập
    public class GetAssignmentSubmissionsQuery : IRequest<Result<List<AssignmentSubmissionDto>>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bài tập cần xem bài nộp
        public Guid AssignmentId { get; set; }
    }
}