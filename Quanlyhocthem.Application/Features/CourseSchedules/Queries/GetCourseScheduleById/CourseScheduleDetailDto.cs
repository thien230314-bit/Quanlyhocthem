namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetCourseScheduleById
{
    // DTO chi tiết lịch học
    public class CourseScheduleDetailDto
    {
        public Guid CourseScheduleId { get; set; }

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public string SubjectName { get; set; } = string.Empty;

        public Guid TeacherId { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public Guid ClassroomId { get; set; }

        public string ClassroomName { get; set; } = string.Empty;

        public string? ClassroomCode { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}