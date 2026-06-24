namespace Quanlyhocthem.Application.Features.CourseSchedules.Queries.GetStudentWeeklySchedule
{
    // DTO thời khóa biểu tuần của học viên
    public class StudentWeeklyScheduleDto
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

        public string? Note { get; set; }
    }
}