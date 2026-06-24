using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.OpenAttendanceSession
{
    // Command để giảng viên mở ca điểm danh cho một lớp học
    public class OpenAttendanceSessionCommand : IRequest<Result<OpenAttendanceSessionResultDto>>
    {
        // Id User của giảng viên lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id lớp học cần mở điểm danh
        public Guid CourseId { get; set; }

        // Số phút ca điểm danh còn hiệu lực
        public int DurationMinutes { get; set; } = 15;

        // Ghi chú của giảng viên
        public string? Note { get; set; }
    }
}