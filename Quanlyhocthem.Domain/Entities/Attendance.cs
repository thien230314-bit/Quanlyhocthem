using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Attendance là bản ghi điểm danh của một học viên trong một ca điểm danh
    public class Attendance : BaseEntity
    {
        // Ca điểm danh
        public Guid AttendanceSessionId { get; private set; }

        public AttendanceSession AttendanceSession { get; private set; } = null!;

        // Enrollment cho biết học viên nào thuộc lớp nào
        public Guid EnrollmentId { get; private set; }

        public Enrollment Enrollment { get; private set; } = null!;

        // Trạng thái điểm danh: Present, Absent, Late, Excused
        public AttendanceStatus Status { get; private set; }

        // Phương thức điểm danh: Code, QR, Manual
        public string Method { get; private set; } = string.Empty;

        // Thời gian học viên điểm danh
        public DateTime? CheckedInAt { get; private set; }

        // Ghi chú
        public string? Note { get; private set; }

        private Attendance()
        {
        }

        public Attendance(
            Guid attendanceSessionId,
            Guid enrollmentId,
            AttendanceStatus status,
            string method,
            DateTime? checkedInAt,
            string? note)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentException("Phương thức điểm danh không được để trống.");

            AttendanceSessionId = attendanceSessionId;
            EnrollmentId = enrollmentId;
            Status = status;
            Method = method.Trim();
            CheckedInAt = checkedInAt;
            Note = note?.Trim();
        }

        // Học viên tự điểm danh bằng mã hoặc QR
        public void StudentCheckIn(string method, DateTime checkedInAt)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentException("Phương thức điểm danh không được để trống.");

            Status = AttendanceStatus.Present;
            Method = method.Trim();
            CheckedInAt = checkedInAt;
            Note = null;
            SetUpdatedAt();
        }

        // Giảng viên sửa điểm danh thủ công
        public void TeacherUpdate(AttendanceStatus status, string? note)
        {
            Status = status;
            Method = "Manual";

            if (status == AttendanceStatus.Present || status == AttendanceStatus.Late)
            {
                CheckedInAt ??= DateTime.UtcNow;
            }

            if (status == AttendanceStatus.Absent || status == AttendanceStatus.Excused)
            {
                CheckedInAt = null;
            }

            Note = note?.Trim();
            SetUpdatedAt();
        }
    }
}