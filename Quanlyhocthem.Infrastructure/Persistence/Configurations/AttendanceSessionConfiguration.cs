using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng AttendanceSessions
    public class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
    {
        public void Configure(EntityTypeBuilder<AttendanceSession> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Mã điểm danh bắt buộc
            builder.Property(x => x.Code)
                .HasMaxLength(20)
                .IsRequired();

            // Mỗi mã điểm danh không được trùng
            builder.HasIndex(x => x.Code)
                .IsUnique();

            // Thời gian mở điểm danh
            builder.Property(x => x.OpenedAt)
                .IsRequired();

            // Thời gian hết hạn
            builder.Property(x => x.ExpiredAt)
                .IsRequired();

            // Trạng thái còn mở hay đã đóng
            builder.Property(x => x.IsOpen)
                .IsRequired();

            // Ghi chú
            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            // Một ca điểm danh thuộc một lớp học
            // Phải trỏ về Course.AttendanceSessions để EF không sinh CourseId1
            builder.HasOne(x => x.Course)
                .WithMany(x => x.AttendanceSessions)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một ca điểm danh do một giảng viên mở
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}