using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Payrolls
    public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tháng tính lương
            builder.Property(x => x.Month)
                .IsRequired();

            // Năm tính lương
            builder.Property(x => x.Year)
                .IsRequired();

            // Số buổi dạy
            builder.Property(x => x.TeachingSessions)
                .IsRequired();

            // Lương cơ bản
            builder.Property(x => x.BaseSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Tiền mỗi buổi dạy
            builder.Property(x => x.TeachingSessionAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Tiền thưởng
            builder.Property(x => x.BonusAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Tiền trừ
            builder.Property(x => x.DeductionAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Tổng lương
            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Trạng thái bảng lương
            builder.Property(x => x.Status)
                .IsRequired();

            // Ghi chú
            builder.Property(x => x.Note)
                .HasMaxLength(1000);

            // Một bảng lương thuộc một giảng viên
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một giảng viên chỉ có một bảng lương trong cùng tháng/năm
            builder.HasIndex(x => new { x.TeacherId, x.Month, x.Year })
                .IsUnique();

            // Index để lọc bảng lương theo tháng/năm
            builder.HasIndex(x => new { x.Month, x.Year });

            // Index để lọc theo trạng thái
            builder.HasIndex(x => x.Status);
        }
    }
}