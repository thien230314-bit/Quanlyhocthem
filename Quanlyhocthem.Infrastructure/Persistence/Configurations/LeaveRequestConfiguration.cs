using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng LeaveRequests
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Ngày xin nghỉ
            builder.Property(x => x.LeaveDate)
                .IsRequired();

            // Lý do xin nghỉ
            builder.Property(x => x.Reason)
                .HasMaxLength(1000)
                .IsRequired();

            // Trạng thái đơn
            builder.Property(x => x.Status)
                .IsRequired();

            // Ghi chú của giảng viên
            builder.Property(x => x.TeacherNote)
                .HasMaxLength(1000);

            // Ghi chú của Admin
            builder.Property(x => x.AdminNote)
                .HasMaxLength(1000);

            // Một đơn thuộc một học viên
            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một đơn thuộc một lớp học
            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Giảng viên xử lý đơn
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admin xử lý đơn
            builder.HasOne(x => x.Admin)
                .WithMany()
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không cho học viên gửi trùng đơn cùng lớp, cùng ngày nếu đơn chưa bị hủy/từ chối
            builder.HasIndex(x => new { x.StudentId, x.CourseId, x.LeaveDate });

            // Index để Teacher/Admin lọc đơn nhanh
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CourseId);
            builder.HasIndex(x => x.StudentId);
        }
    }
}