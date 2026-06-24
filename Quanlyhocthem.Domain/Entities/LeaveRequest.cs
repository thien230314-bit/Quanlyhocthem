using Quanlyhocthem.Domain.Common;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Domain.Entities
{
    // LeaveRequest là đơn xin nghỉ của học viên theo từng lớp học
    public class LeaveRequest : BaseEntity
    {
        // Học viên gửi đơn
        public Guid StudentId { get; private set; }

        public Student Student { get; private set; } = null!;

        // Lớp học mà học viên xin nghỉ
        public Guid CourseId { get; private set; }

        public Course Course { get; private set; } = null!;

        // Ngày xin nghỉ
        public DateTime LeaveDate { get; private set; }

        // Lý do xin nghỉ
        public string Reason { get; private set; } = string.Empty;

        // Trạng thái đơn
        public LeaveRequestStatus Status { get; private set; }

        // Giảng viên xử lý
        public Guid? TeacherId { get; private set; }

        public User? Teacher { get; private set; }

        // Thời gian giảng viên xử lý
        public DateTime? TeacherReviewedAt { get; private set; }

        // Ghi chú của giảng viên
        public string? TeacherNote { get; private set; }

        // Admin xử lý
        public Guid? AdminId { get; private set; }

        public User? Admin { get; private set; }

        // Thời gian Admin xử lý
        public DateTime? AdminReviewedAt { get; private set; }

        // Ghi chú của Admin
        public string? AdminNote { get; private set; }

        private LeaveRequest()
        {
        }

        public LeaveRequest(Guid studentId, Guid courseId, DateTime leaveDate, string reason)
        {
            if (studentId == Guid.Empty)
                throw new ArgumentException("Id học viên không hợp lệ.");

            if (courseId == Guid.Empty)
                throw new ArgumentException("Id lớp học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Lý do xin nghỉ không được để trống.");

            StudentId = studentId;
            CourseId = courseId;
            LeaveDate = leaveDate.Date;
            Reason = reason.Trim();
            Status = LeaveRequestStatus.PendingTeacher;
        }

        // Học viên sửa đơn khi đơn còn chờ giảng viên duyệt
        public void Update(DateTime leaveDate, string reason)
        {
            if (Status != LeaveRequestStatus.PendingTeacher)
                throw new InvalidOperationException("Chỉ được sửa đơn khi đang chờ giảng viên duyệt.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Lý do xin nghỉ không được để trống.");

            LeaveDate = leaveDate.Date;
            Reason = reason.Trim();

            SetUpdatedAt();
        }

        // Học viên hủy đơn khi đơn chưa được xử lý
        public void Cancel()
        {
            if (Status != LeaveRequestStatus.PendingTeacher)
                throw new InvalidOperationException("Chỉ được hủy đơn khi đang chờ giảng viên duyệt.");

            Status = LeaveRequestStatus.Cancelled;

            SetUpdatedAt();
        }

        // Giảng viên duyệt đơn
        public void TeacherApprove(Guid teacherId, string? teacherNote, DateTime reviewedAt)
        {
            if (Status != LeaveRequestStatus.PendingTeacher)
                throw new InvalidOperationException("Chỉ được duyệt đơn đang chờ giảng viên xử lý.");

            TeacherId = teacherId;
            TeacherReviewedAt = reviewedAt;
            TeacherNote = string.IsNullOrWhiteSpace(teacherNote) ? null : teacherNote.Trim();
            Status = LeaveRequestStatus.ApprovedByTeacher;

            SetUpdatedAt();
        }

        // Giảng viên từ chối đơn
        public void TeacherReject(Guid teacherId, string? teacherNote, DateTime reviewedAt)
        {
            if (Status != LeaveRequestStatus.PendingTeacher)
                throw new InvalidOperationException("Chỉ được từ chối đơn đang chờ giảng viên xử lý.");

            TeacherId = teacherId;
            TeacherReviewedAt = reviewedAt;
            TeacherNote = string.IsNullOrWhiteSpace(teacherNote) ? null : teacherNote.Trim();
            Status = LeaveRequestStatus.RejectedByTeacher;

            SetUpdatedAt();
        }

        // Admin xác nhận đơn đã được giảng viên duyệt
        public void AdminApprove(Guid adminId, string? adminNote, DateTime reviewedAt)
        {
            if (Status != LeaveRequestStatus.ApprovedByTeacher)
                throw new InvalidOperationException("Admin chỉ xử lý đơn đã được giảng viên duyệt.");

            AdminId = adminId;
            AdminReviewedAt = reviewedAt;
            AdminNote = string.IsNullOrWhiteSpace(adminNote) ? null : adminNote.Trim();
            Status = LeaveRequestStatus.ApprovedByAdmin;

            SetUpdatedAt();
        }

        // Admin từ chối đơn đã được giảng viên duyệt
        public void AdminReject(Guid adminId, string? adminNote, DateTime reviewedAt)
        {
            if (Status != LeaveRequestStatus.ApprovedByTeacher)
                throw new InvalidOperationException("Admin chỉ xử lý đơn đã được giảng viên duyệt.");

            AdminId = adminId;
            AdminReviewedAt = reviewedAt;
            AdminNote = string.IsNullOrWhiteSpace(adminNote) ? null : adminNote.Trim();
            Status = LeaveRequestStatus.RejectedByAdmin;

            SetUpdatedAt();
        }
    }
}