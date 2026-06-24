using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Submissions.Commands.SubmitAssignment
{
    // Command để học viên nộp bài tập
    public class SubmitAssignmentCommand : IRequest<Result<Guid>>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id bài tập cần nộp
        public Guid AssignmentId { get; set; }

        // Nội dung bài nộp
        public string Content { get; set; } = string.Empty;

        // Tên file bài nộp nếu có
        public string? SubmittedFileName { get; set; }

        // Đường dẫn file bài nộp nếu có
        public string? SubmittedFileUrl { get; set; }
    }
}