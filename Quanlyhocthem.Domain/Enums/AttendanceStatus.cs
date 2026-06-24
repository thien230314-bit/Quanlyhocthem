namespace Quanlyhocthem.Domain.Enums
{
    // Trạng thái điểm danh của học viên trong một buổi học
    public enum AttendanceStatus
    {
        // Có mặt
        Present = 1,

        // Vắng mặt
        Absent = 2,

        // Đi trễ
        Late = 3,

        // Vắng có phép
        Excused = 4
    }
}