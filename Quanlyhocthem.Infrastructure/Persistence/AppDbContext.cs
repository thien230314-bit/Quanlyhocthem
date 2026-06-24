using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence
{
    // DbContext chính của hệ thống
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Bảng tài khoản
        public DbSet<User> Users => Set<User>();

        // Bảng refresh token
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // Bảng học viên
        public DbSet<Student> Students => Set<Student>();

        // Bảng khóa học viên
        public DbSet<Batch> Batches => Set<Batch>();

        // Bảng môn học
        public DbSet<Subject> Subjects => Set<Subject>();

        // Bảng phòng học
        public DbSet<Classroom> Classrooms => Set<Classroom>();

        // Bảng lớp học
        public DbSet<Course> Courses => Set<Course>();

        // Bảng lịch học / lịch dạy
        public DbSet<CourseSchedule> CourseSchedules => Set<CourseSchedule>();

        // Bảng đăng ký lớp học
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        // Bảng hóa đơn học phí
        public DbSet<TuitionInvoice> TuitionInvoices => Set<TuitionInvoice>();

        // Bảng điểm danh
        public DbSet<Attendance> Attendances => Set<Attendance>();

        // Bảng ca điểm danh
        public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();

        // Bảng bài tập
        public DbSet<Assignment> Assignments => Set<Assignment>();

        // Bảng bài nộp
        public DbSet<Submission> Submissions => Set<Submission>();

        // Bảng điểm bài nộp
        public DbSet<Grade> Grades => Set<Grade>();

        // Bảng thông báo
        public DbSet<Notification> Notifications => Set<Notification>();

        // Bảng đơn xin nghỉ
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        // Bảng lương giảng viên
        public DbSet<Payroll> Payrolls => Set<Payroll>();

        // Bảng nhật ký hệ thống
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tự động apply toàn bộ configuration trong Infrastructure
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}