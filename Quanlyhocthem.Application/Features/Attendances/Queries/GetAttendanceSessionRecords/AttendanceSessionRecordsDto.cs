namespace Quanlyhocthem.Application.Features.Attendances.Queries.GetAttendanceSessionRecords
{
    // DTO trả thông tin ca điểm danh và danh sách học viên
    public class AttendanceSessionRecordsDto
    {
        // Id ca điểm danh
        public Guid AttendanceSessionId { get; set; }

        // Tên lớp
        public string CourseName { get; set; } = string.Empty;

        // Mã điểm danh
        public string AttendanceCode { get; set; } = string.Empty;

        // Thời gian mở
        public DateTime OpenedAt { get; set; }

        // Thời gian hết hạn
        public DateTime ExpiredAt { get; set; }

        // Ca còn mở không
        public bool IsOpen { get; set; }

        // Tổng số học viên
        public int TotalStudents { get; set; }

        // Số học viên có mặt
        public int PresentCount { get; set; }

        // Số học viên vắng
        public int AbsentCount { get; set; }

        // Danh sách bản ghi điểm danh
        public List<AttendanceRecordDto> Records { get; set; } = new List<AttendanceRecordDto>();
    }
}