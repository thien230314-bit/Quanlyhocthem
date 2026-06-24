namespace Quanlyhocthem.Application.Features.Attendances.Commands.OpenAttendanceSession
{
    // DTO trả kết quả sau khi giảng viên mở ca điểm danh
    public class OpenAttendanceSessionResultDto
    {
        // Id ca điểm danh
        public Guid AttendanceSessionId { get; set; }

        // Mã điểm danh để học viên nhập
        public string AttendanceCode { get; set; } = string.Empty;

        // Thời gian mở điểm danh
        public DateTime OpenedAt { get; set; }

        // Thời gian hết hạn
        public DateTime ExpiredAt { get; set; }

        // Số học viên trong lớp được tạo bản ghi điểm danh
        public int TotalStudents { get; set; }
    }
}