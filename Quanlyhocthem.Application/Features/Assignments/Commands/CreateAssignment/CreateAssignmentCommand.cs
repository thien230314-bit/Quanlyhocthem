using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.CreateAssignment
{
    // Command để giảng viên tạo bài tập cho một lớp học
    public class CreateAssignmentCommand : IRequest<Result<Guid>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id lớp học cần giao bài tập
        public Guid CourseId { get; set; }

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