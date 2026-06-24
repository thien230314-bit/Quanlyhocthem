using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Common.Interfaces
{
    // Interface DbContext dùng cho tầng Application
    public interface IAppDbContext
    {
        // Bảng tài khoản
        DbSet<User> Users { get; }

        // Bảng refresh token
        DbSet<RefreshToken> RefreshTokens { get; }

        // Bảng học viên
        DbSet<Student> Students { get; }

        // Bảng khóa học viên
        DbSet<Batch> Batches { get; }

        // Bảng môn học
        DbSet<Subject> Subjects { get; }

        // Bảng phòng học
        DbSet<Classroom> Classrooms { get; }

        // Bảng lớp học
        DbSet<Course> Courses { get; }

        // Bảng lịch học / lịch dạy
        DbSet<CourseSchedule> CourseSchedules { get; }

        // Bảng đăng ký lớp học
        DbSet<Enrollment> Enrollments { get; }

        // Bảng hóa đơn học phí
        DbSet<TuitionInvoice> TuitionInvoices { get; }

        // Bảng điểm danh
        DbSet<Attendance> Attendances { get; }

        // Bảng ca điểm danh
        DbSet<AttendanceSession> AttendanceSessions { get; }

        // Bảng bài tập
        DbSet<Assignment> Assignments { get; }

        // Bảng bài nộp
        DbSet<Submission> Submissions { get; }

        // Bảng điểm bài nộp
        DbSet<Grade> Grades { get; }

        // Bảng thông báo
        DbSet<Notification> Notifications { get; }

        // Bảng đơn xin nghỉ
        DbSet<LeaveRequest> LeaveRequests { get; }

        // Bảng lương giảng viên
        DbSet<Payroll> Payrolls { get; }

        // Bảng nhật ký hệ thống
        DbSet<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}