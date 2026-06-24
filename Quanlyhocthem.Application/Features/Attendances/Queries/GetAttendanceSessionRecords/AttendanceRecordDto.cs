using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Attendances.Queries.GetAttendanceSessionRecords
{
    // DTO trả từng bản ghi điểm danh trong một ca
    public class AttendanceRecordDto
    {
        // Id bản ghi điểm danh
        public Guid AttendanceId { get; set; }

        // Id ca điểm danh
        public Guid AttendanceSessionId { get; set; }

        // Id đăng ký lớp
        public Guid EnrollmentId { get; set; }

        // Mã học viên
        public string StudentCode { get; set; } = string.Empty;

        // Họ tên học viên
        public string StudentName { get; set; } = string.Empty;

        // Email học viên
        public string Email { get; set; } = string.Empty;

        // Trạng thái điểm danh
        public AttendanceStatus Status { get; set; }

        // Phương thức điểm danh
        public string Method { get; set; } = string.Empty;

        // Thời gian điểm danh
        public DateTime? CheckedInAt { get; set; }

        // Ghi chú
        public string? Note { get; set; }
    }
}