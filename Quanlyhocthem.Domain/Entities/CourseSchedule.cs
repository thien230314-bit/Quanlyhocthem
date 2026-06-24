using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // CourseSchedule là lịch học cố định theo tuần của một lớp học
    public class CourseSchedule : BaseEntity
    {
        // Lớp học có lịch
        public Guid CourseId { get; private set; }

        public Course Course { get; private set; } = null!;

        // Giảng viên dạy ca này
        public Guid TeacherId { get; private set; }

        public User Teacher { get; private set; } = null!;

        // Phòng học
        public Guid ClassroomId { get; private set; }

        public Classroom Classroom { get; private set; } = null!;

        // Thứ trong tuần: Monday, Tuesday, Wednesday...
        public DayOfWeek DayOfWeek { get; private set; }

        // Giờ bắt đầu học
        public TimeSpan StartTime { get; private set; }

        // Giờ kết thúc học
        public TimeSpan EndTime { get; private set; }

        // Ngày bắt đầu áp dụng lịch
        public DateTime EffectiveFrom { get; private set; }

        // Ngày kết thúc áp dụng lịch, có thể null nếu chưa xác định
        public DateTime? EffectiveTo { get; private set; }

        // Trạng thái lịch còn hoạt động hay không
        public bool IsActive { get; private set; } = true;

        // Ghi chú
        public string? Note { get; private set; }

        private CourseSchedule()
        {
        }

        public CourseSchedule(
            Guid courseId,
            Guid teacherId,
            Guid classroomId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            DateTime effectiveFrom,
            DateTime? effectiveTo,
            string? note)
        {
            if (courseId == Guid.Empty)
                throw new ArgumentException("Id lớp học không hợp lệ.");

            if (teacherId == Guid.Empty)
                throw new ArgumentException("Id giảng viên không hợp lệ.");

            if (classroomId == Guid.Empty)
                throw new ArgumentException("Id phòng học không hợp lệ.");

            if (startTime >= endTime)
                throw new ArgumentException("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            if (effectiveTo.HasValue && effectiveTo.Value.Date < effectiveFrom.Date)
                throw new ArgumentException("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            CourseId = courseId;
            TeacherId = teacherId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            EffectiveFrom = effectiveFrom.Date;
            EffectiveTo = effectiveTo?.Date;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            IsActive = true;
        }

        // Cập nhật lịch học
        public void Update(
            Guid teacherId,
            Guid classroomId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            DateTime effectiveFrom,
            DateTime? effectiveTo,
            string? note)
        {
            if (teacherId == Guid.Empty)
                throw new ArgumentException("Id giảng viên không hợp lệ.");

            if (classroomId == Guid.Empty)
                throw new ArgumentException("Id phòng học không hợp lệ.");

            if (startTime >= endTime)
                throw new ArgumentException("Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");

            if (effectiveTo.HasValue && effectiveTo.Value.Date < effectiveFrom.Date)
                throw new ArgumentException("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            TeacherId = teacherId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            EffectiveFrom = effectiveFrom.Date;
            EffectiveTo = effectiveTo?.Date;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

            SetUpdatedAt();
        }

        // Ngưng hoạt động lịch học
        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }

        // Kích hoạt lại lịch học
        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }
    }
}