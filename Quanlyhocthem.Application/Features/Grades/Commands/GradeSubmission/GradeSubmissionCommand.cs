using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Grades.Commands.GradeSubmission
{
    // Command để giảng viên chấm điểm bài nộp
    public class GradeSubmissionCommand : IRequest<Result<Guid>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bài nộp cần chấm
        public Guid SubmissionId { get; set; }

        // Điểm số
        public decimal Score { get; set; }

        // Nhận xét của giảng viên
        public string? Feedback { get; set; }
    }
}