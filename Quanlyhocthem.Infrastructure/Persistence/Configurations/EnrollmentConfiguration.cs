using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Enrollments
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Một Enrollment thuộc một Course
            builder.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một Enrollment thuộc một Student
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không cho một học viên đăng ký trùng một khóa học
            builder.HasIndex(x => new { x.StudentId, x.CourseId })
                .IsUnique();

            // Trạng thái đăng ký: Active, Cancelled, Completed
            builder.Property(x => x.Status)
                .IsRequired();
        }
    }
}