using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Attendances
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Trạng thái điểm danh bắt buộc
            builder.Property(x => x.Status)
                .IsRequired();

            // Phương thức điểm danh bắt buộc
            builder.Property(x => x.Method)
                .HasMaxLength(50)
                .IsRequired();

            // Ghi chú
            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            // Một bản ghi điểm danh thuộc một ca điểm danh
            builder.HasOne(x => x.AttendanceSession)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.AttendanceSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Một bản ghi điểm danh thuộc một Enrollment
            // Phải trỏ về Enrollment.Attendances để EF không sinh EnrollmentId1
            builder.HasOne(x => x.Enrollment)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một học viên chỉ có một bản ghi điểm danh trong một ca
            builder.HasIndex(x => new { x.AttendanceSessionId, x.EnrollmentId })
                .IsUnique();
        }
    }
}