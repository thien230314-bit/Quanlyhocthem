using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Grades
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Điểm số
            builder.Property(x => x.Score)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            // Nhận xét của giảng viên
            builder.Property(x => x.Feedback)
                .HasMaxLength(2000);

            // Thời gian chấm điểm
            builder.Property(x => x.GradedAt)
                .IsRequired();

            // Một Submission chỉ có một Grade
            builder.HasOne(x => x.Submission)
                .WithOne(x => x.Grade)
                .HasForeignKey<Grade>(x => x.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Giảng viên chấm điểm
            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Không cho một bài nộp bị chấm trùng nhiều lần
            builder.HasIndex(x => x.SubmissionId)
                .IsUnique();
        }
    }
}