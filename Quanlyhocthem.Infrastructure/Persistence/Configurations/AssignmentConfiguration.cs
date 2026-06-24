using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Assignments
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tiêu đề bài tập bắt buộc
            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            // Mô tả bài tập
            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired();

            // Hạn nộp bài
            builder.Property(x => x.DueDate)
                .IsRequired();

            // Điểm tối đa
            builder.Property(x => x.MaxScore)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            // Tên file đính kèm
            builder.Property(x => x.AttachmentFileName)
                .HasMaxLength(255);

            // Đường dẫn file đính kèm
            builder.Property(x => x.AttachmentUrl)
                .HasMaxLength(1000);

            // Trạng thái hoạt động
            builder.Property(x => x.IsActive)
                .IsRequired();

            // Một Assignment thuộc một Course
            // Phải trỏ về Course.Assignments để EF không sinh CourseId1
            builder.HasOne(x => x.Course)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một Assignment do một Teacher tạo
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}