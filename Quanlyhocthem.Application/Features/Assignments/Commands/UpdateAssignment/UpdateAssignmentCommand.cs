using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.UpdateAssignment
{
    // Command để giảng viên cập nhật bài tập đã giao
    public class UpdateAssignmentCommand : IRequest<Result>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bài tập cần cập nhật
        public Guid AssignmentId { get; set; }

        // Tiêu đề bài tập
        public string Title { get; set; } = string.Empty;

        // Mô tả / nội dung bài tập
        public string Description { get; set; } = string.Empty;

        // Hạn nộp bài
        public DateTime DueDate { get; set; }

        // Điểm tối đa
        public decimal MaxScore { get; set; }

        // Tên file đính kèm nếu có
        public string? AttachmentFileName { get; set; }

        // Đường dẫn file đính kèm nếu có
        public string? AttachmentUrl { get; set; }
    }
}