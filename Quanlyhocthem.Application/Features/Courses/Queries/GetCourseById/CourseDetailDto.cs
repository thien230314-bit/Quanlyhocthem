namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourseById
{
    // DTO chi tiết lớp học
    public class CourseDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal TuitionFee { get; set; }

        public int MaxStudents { get; set; }

        public int CurrentStudents { get; set; }

        public Guid SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;

        public Guid BatchId { get; set; }

        public string BatchName { get; set; } = string.Empty;

        public Guid? ClassroomId { get; set; }

        public string? ClassroomName { get; set; }

        public Guid? TeacherId { get; set; }

        public string? TeacherName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public int TotalAssignments { get; set; }

        public int TotalAttendanceSessions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}