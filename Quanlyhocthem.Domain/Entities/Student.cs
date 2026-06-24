using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Entity Student lưu thông tin riêng của học viên
    // Không có ParentName, ParentPhone vì hệ thống chỉ có 3 role: Admin, Teacher, Student
    public class Student : BaseEntity
    {
        // Mã học viên, ví dụ: ST001
        public string StudentCode { get; private set; } = string.Empty;

        // Ngày sinh của học viên
        public DateTime DateOfBirth { get; private set; }

        // Id tài khoản User của học viên
        public Guid UserId { get; private set; }

        // Navigation tới User
        public User User { get; private set; } = null!;

        // Id khóa học viên, ví dụ: Khóa 2025
        // Dùng để lọc Course cho đúng khóa của học viên
        public Guid BatchId { get; private set; }

        // Navigation tới Batch
        public Batch Batch { get; private set; } = null!;

        // Danh sách lớp học mà học viên đã đăng ký
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        // Constructor rỗng cho Entity Framework Core
        private Student()
        {
        }

        // Constructor tạo hồ sơ học viên
        public Student(string studentCode, DateTime dateOfBirth, Guid userId, Guid batchId)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                throw new ArgumentException("Mã học viên không được để trống.");

            StudentCode = studentCode.Trim();
            DateOfBirth = dateOfBirth;
            UserId = userId;
            BatchId = batchId;
        }

        // Cập nhật thông tin học viên
        public void UpdateInfo(string studentCode, DateTime dateOfBirth, Guid batchId)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                throw new ArgumentException("Mã học viên không được để trống.");

            StudentCode = studentCode.Trim();
            DateOfBirth = dateOfBirth;
            BatchId = batchId;
            SetUpdatedAt();
        }
    }
}