using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // Entity Enrollment là bảng trung gian giữa Student và Course
    // Một học viên đăng ký một khóa học thì tạo một Enrollment
    public class Enrollment : BaseEntity
    {
        // Khóa học được đăng ký
        public Guid CourseId { get; private set; }

        // Navigation property tới Course
        public Course Course { get; private set; } = null!;

        // Học viên đăng ký
        public Guid StudentId { get; private set; }

        // Navigation property tới Student
        public Student Student { get; private set; } = null!;

        // Ngày đăng ký
        public DateTime EnrollmentDate { get; private set; } = DateTime.UtcNow;

        // Trạng thái đăng ký
        public EnrollmentStatus Status { get; private set; } = EnrollmentStatus.Active;

        // Danh sách điểm danh của học viên trong khóa học này
        public ICollection<Attendance> Attendances { get; private set; } = new List<Attendance>();

        // Constructor rỗng cho Entity Framework Core
        private Enrollment()
        {
        }

        // Constructor tạo đăng ký khóa học
        public Enrollment(Guid courseId, Guid studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
            EnrollmentDate = DateTime.UtcNow;
            Status = EnrollmentStatus.Active;
        }

        // Hủy đăng ký khóa học
        public void Cancel()
        {
            Status = EnrollmentStatus.Cancelled;
            SetUpdatedAt();
        }

        // Đánh dấu hoàn thành khóa học
        public void Complete()
        {
            Status = EnrollmentStatus.Completed;
            SetUpdatedAt();
        }
    }
}