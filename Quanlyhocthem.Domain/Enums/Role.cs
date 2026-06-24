namespace Quanlyhocthem.Domain.Enums
{
    // Hệ thống chỉ có 3 vai trò đúng theo giao diện web app
    // Không có Parent
    public enum Role
    {
        // Admin quản lý tài khoản, khóa học, học phí, hệ thống
        Admin = 0,

        // Teacher là giảng viên, quản lý lớp được giao
        Teacher = 1,

        // Student là học viên, đăng ký môn học và xem thông tin học tập
        Student = 2
    }
}