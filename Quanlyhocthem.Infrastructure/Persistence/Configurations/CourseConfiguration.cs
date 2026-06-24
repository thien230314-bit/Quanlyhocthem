using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Courses
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tên lớp học bắt buộc
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            // Mô tả lớp học
            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // Học phí dùng decimal để tránh sai số tiền
            builder.Property(x => x.TuitionFee)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Sĩ số tối đa
            builder.Property(x => x.MaxStudents)
                .IsRequired();

            // Trạng thái lớp học
            builder.Property(x => x.IsActive)
                .IsRequired();

            // Course thuộc Subject
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course thuộc Batch
            builder.HasOne(x => x.Batch)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course có thể có Classroom
            builder.HasOne(x => x.Classroom)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull);

            // Course có thể được gán cho Teacher
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}