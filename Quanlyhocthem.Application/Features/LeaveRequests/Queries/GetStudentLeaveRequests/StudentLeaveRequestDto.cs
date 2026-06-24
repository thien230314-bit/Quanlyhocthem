using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetStudentLeaveRequests
{
    // DTO danh sách đơn xin nghỉ của học viên
    public class StudentLeaveRequestDto
    {
        public Guid LeaveRequestId { get; set; }

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public DateTime LeaveDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public LeaveRequestStatus Status { get; set; }

        public string? TeacherName { get; set; }

        public DateTime? TeacherReviewedAt { get; set; }

        public string? TeacherNote { get; set; }

        public string? AdminName { get; set; }

        public DateTime? AdminReviewedAt { get; set; }

        public string? AdminNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}