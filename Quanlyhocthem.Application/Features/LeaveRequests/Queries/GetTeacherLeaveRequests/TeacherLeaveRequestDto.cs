using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.LeaveRequests.Queries.GetTeacherLeaveRequests
{
    // DTO danh sách đơn xin nghỉ cho giảng viên
    public class TeacherLeaveRequestDto
    {
        public Guid LeaveRequestId { get; set; }

        public Guid StudentId { get; set; }

        public string StudentCode { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public DateTime LeaveDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public LeaveRequestStatus Status { get; set; }

        public DateTime? TeacherReviewedAt { get; set; }

        public string? TeacherNote { get; set; }

        public DateTime? AdminReviewedAt { get; set; }

        public string? AdminNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}