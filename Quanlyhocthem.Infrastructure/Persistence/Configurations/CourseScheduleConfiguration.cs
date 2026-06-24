using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng CourseSchedules
    public class CourseScheduleConfiguration : IEntityTypeConfiguration<CourseSchedule>
    {
        public void Configure(EntityTypeBuilder<CourseSchedule> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Thứ trong tuần
            builder.Property(x => x.DayOfWeek)
                .IsRequired();

            // Giờ bắt đầu
            builder.Property(x => x.StartTime)
                .IsRequired();

            // Giờ kết thúc
            builder.Property(x => x.EndTime)
                .IsRequired();

            // Ngày bắt đầu áp dụng
            builder.Property(x => x.EffectiveFrom)
                .IsRequired();

            // Trạng thái hoạt động
            builder.Property(x => x.IsActive)
                .IsRequired();

            // Ghi chú
            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            // Một lịch học thuộc một lớp học
            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một lịch học có một giảng viên dạy
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một lịch học diễn ra tại một phòng học
            builder.HasOne(x => x.Classroom)
                .WithMany()
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index để lọc lịch theo lớp
            builder.HasIndex(x => x.CourseId);

            // Index để lọc lịch theo giảng viên
            builder.HasIndex(x => x.TeacherId);

            // Index để lọc lịch theo phòng học
            builder.HasIndex(x => x.ClassroomId);

            // Index để lọc lịch theo thứ
            builder.HasIndex(x => x.DayOfWeek);
        }
    }
}