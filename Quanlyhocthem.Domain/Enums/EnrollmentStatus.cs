namespace Quanlyhocthem.Domain.Enums
{
    // Trạng thái đăng ký môn học của học viên
    public enum EnrollmentStatus
    {
        // Đang học / đăng ký còn hiệu lực
        Active = 1,

        // Học viên đã hủy đăng ký
        Cancelled = 2,

        // Học viên đã hoàn thành khóa học
        Completed = 3
    }
}