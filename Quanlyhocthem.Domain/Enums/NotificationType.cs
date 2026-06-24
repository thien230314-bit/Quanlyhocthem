namespace Quanlyhocthem.Domain.Enums
{
    // Loại thông báo trong hệ thống
    public enum NotificationType
    {
        General = 0,

        // Giảng viên giao bài tập mới
        AssignmentCreated = 1,

        // Giảng viên chấm điểm bài nộp
        SubmissionGraded = 2,

        // Liên quan đến điểm danh
        Attendance = 3,

        // Liên quan đến xin nghỉ
        LeaveRequest = 4,

        // Liên quan đến học phí
        Tuition = 5
    }
}