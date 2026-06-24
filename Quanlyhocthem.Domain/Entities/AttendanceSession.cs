using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // AttendanceSession là ca điểm danh do giảng viên mở cho một lớp học
    public class AttendanceSession : BaseEntity
    {
        // Lớp học được mở điểm danh
        public Guid CourseId { get; private set; }

        public Course Course { get; private set; } = null!;

        // Mã điểm danh để học viên nhập
        public string Code { get; private set; } = string.Empty;

        // Thời gian mở điểm danh
        public DateTime OpenedAt { get; private set; }

        // Thời gian hết hạn điểm danh
        public DateTime ExpiredAt { get; private set; }

        // Người mở điểm danh, chính là Teacher UserId
        public Guid TeacherId { get; private set; }

        public User Teacher { get; private set; } = null!;

        // Ca điểm danh còn mở không
        public bool IsOpen { get; private set; } = true;

        // Ghi chú của giảng viên
        public string? Note { get; private set; }

        // Danh sách bản ghi điểm danh trong ca này
        public ICollection<Attendance> Attendances { get; private set; } = new List<Attendance>();

        private AttendanceSession()
        {
        }

        public AttendanceSession(
            Guid courseId,
            Guid teacherId,
            string code,
            DateTime openedAt,
            DateTime expiredAt,
            string? note)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Mã điểm danh không được để trống.");

            if (expiredAt <= openedAt)
                throw new ArgumentException("Thời gian hết hạn phải lớn hơn thời gian mở điểm danh.");

            CourseId = courseId;
            TeacherId = teacherId;
            Code = code.Trim().ToUpper();
            OpenedAt = openedAt;
            ExpiredAt = expiredAt;
            Note = note?.Trim();
            IsOpen = true;
        }

        // Đóng ca điểm danh
        public void Close()
        {
            IsOpen = false;
            SetUpdatedAt();
        }

        // Kiểm tra ca điểm danh còn hiệu lực không
        public bool CanCheckIn(DateTime now)
        {
            return IsOpen && now <= ExpiredAt;
        }
    }
}