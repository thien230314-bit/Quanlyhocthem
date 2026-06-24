using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Attendances.Commands.StudentCheckIn
{
    // Command để học viên điểm danh bằng mã
    public class StudentCheckInCommand : IRequest<Result>
    {
        // Id User của học viên lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Mã điểm danh do giảng viên mở
        public string AttendanceCode { get; set; } = string.Empty;
    }
}