using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Grades.Queries.GetStudentGrades
{
    // Query để học viên xem điểm các bài đã được chấm
    public class GetStudentGradesQuery : IRequest<Result<List<StudentGradeDto>>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }
    }
}