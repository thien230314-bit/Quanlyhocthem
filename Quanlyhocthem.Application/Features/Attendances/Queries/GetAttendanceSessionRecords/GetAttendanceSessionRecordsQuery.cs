using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Attendances.Queries.GetAttendanceSessionRecords
{
    // Query để giảng viên xem kết quả điểm danh của một ca
    public class GetAttendanceSessionRecordsQuery : IRequest<Result<AttendanceSessionRecordsDto>>
    {
        // Id User của giảng viên lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id ca điểm danh
        public Guid AttendanceSessionId { get; set; }
    }
}