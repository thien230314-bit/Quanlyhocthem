namespace Quanlyhocthem.Domain.Enums
{
    // Trạng thái đơn xin nghỉ
    public enum LeaveRequestStatus
    {
        // Học viên vừa gửi, chờ giảng viên duyệt
        PendingTeacher = 0,

        // Giảng viên đã duyệt, chờ Admin xác nhận
        ApprovedByTeacher = 1,

        // Giảng viên từ chối
        RejectedByTeacher = 2,

        // Admin đã xác nhận
        ApprovedByAdmin = 3,

        // Admin từ chối
        RejectedByAdmin = 4,

        // Học viên tự hủy đơn
        Cancelled = 5
    }
}