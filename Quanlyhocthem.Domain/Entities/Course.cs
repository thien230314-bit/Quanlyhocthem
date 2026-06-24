using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Course là lớp học cụ thể mà học viên đăng ký
    // Ví dụ: Toán 12 - Khóa 2025 - Thầy Nam - Phòng A1
    public class Course : BaseEntity
    {
        // Tên lớp học
        public string Name { get; private set; } = string.Empty;

        // Mô tả lớp học
        public string Description { get; private set; } = string.Empty;

        // Học phí của lớp
        public decimal TuitionFee { get; private set; }

        // Sĩ số tối đa
        public int MaxStudents { get; private set; }

        // Môn học của lớp
        public Guid SubjectId { get; private set; }

        public Subject Subject { get; private set; } = null!;

        // Khóa áp dụng cho lớp
        public Guid BatchId { get; private set; }

        public Batch Batch { get; private set; } = null!;

        // Phòng học
        public Guid? ClassroomId { get; private set; }

        public Classroom? Classroom { get; private set; }

        // Giảng viên được phân công
        public Guid? TeacherId { get; private set; }

        public User? Teacher { get; private set; }

        // Ngày bắt đầu lớp
        public DateTime StartDate { get; private set; }

        // Ngày kết thúc lớp
        public DateTime? EndDate { get; private set; }

        // Lớp còn mở đăng ký/học không
        public bool IsActive { get; private set; } = true;

        // Danh sách học viên đăng ký
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        // Danh sách bài tập của lớp
        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();

        // Danh sách ca điểm danh của lớp
        public ICollection<AttendanceSession> AttendanceSessions { get; private set; } = new List<AttendanceSession>();

        private Course()
        {
        }

        public Course(
            string name,
            string description,
            decimal tuitionFee,
            int maxStudents,
            Guid subjectId,
            Guid batchId,
            Guid? classroomId,
            Guid? teacherId,
            DateTime startDate,
            DateTime? endDate)
        {
            UpdateDetails(name, description, tuitionFee, maxStudents, startDate, endDate);

            SubjectId = subjectId;
            BatchId = batchId;
            ClassroomId = classroomId;
            TeacherId = teacherId;
            IsActive = true;
        }

        public void UpdateDetails(
            string name,
            string description,
            decimal tuitionFee,
            int maxStudents,
            DateTime startDate,
            DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên lớp học không được để trống.");

            if (tuitionFee < 0)
                throw new ArgumentException("Học phí không được phép âm.");

            if (maxStudents < 1)
                throw new ArgumentException("Sĩ số tối đa phải lớn hơn 0.");

            if (endDate.HasValue && endDate.Value < startDate)
                throw new ArgumentException("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            TuitionFee = tuitionFee;
            MaxStudents = maxStudents;
            StartDate = startDate;
            EndDate = endDate;
            SetUpdatedAt();
        }

        public void AssignTeacher(Guid? teacherId)
        {
            TeacherId = teacherId;
            SetUpdatedAt();
        }
        
        public void AssignClassroom(Guid? classroomId)
        {
            ClassroomId = classroomId;
            SetUpdatedAt();
        }
        // Cập nhật liên kết môn học, khóa, phòng và giảng viên của lớp
        public void UpdateRelations(
            Guid subjectId,
            Guid batchId,
            Guid? classroomId,
            Guid? teacherId)
        {
            SubjectId = subjectId;
            BatchId = batchId;
            ClassroomId = classroomId;
            TeacherId = teacherId;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}