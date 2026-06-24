using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.UpdateAttendanceRecord
{
    // Command để giảng viên sửa điểm danh thủ công
    public class UpdateAttendanceRecordCommand : IRequest<Result>
    {
        // Id User của giảng viên lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bản ghi điểm danh cần sửa
        public Guid AttendanceId { get; set; }

        // Trạng thái mới
        public AttendanceStatus Status { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}