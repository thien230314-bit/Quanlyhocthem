using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Students
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Mã học viên bắt buộc và không được trùng
            builder.Property(x => x.StudentCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.StudentCode)
                .IsUnique();

            // Một Student liên kết với một User
            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Student>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một Student thuộc một Batch
            builder.HasOne(x => x.Batch)
                .WithMany()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}