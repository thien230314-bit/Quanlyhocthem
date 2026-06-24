using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Submissions
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Nội dung bài nộp
            builder.Property(x => x.Content)
                .HasMaxLength(4000)
                .IsRequired();

            // Tên file bài nộp
            builder.Property(x => x.SubmittedFileName)
                .HasMaxLength(255);

            // Đường dẫn file bài nộp
            builder.Property(x => x.SubmittedFileUrl)
                .HasMaxLength(1000);

            // Thời gian nộp bài
            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            // Trạng thái bài nộp
            builder.Property(x => x.Status)
                .IsRequired();

            // Một Submission thuộc một Assignment
            builder.HasOne(x => x.Assignment)
                .WithMany(x => x.Submissions)
                .HasForeignKey(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Một Submission thuộc một Enrollment
            builder.HasOne(x => x.Enrollment)
                .WithMany()
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một học viên chỉ được có một bài nộp cho một bài tập
            builder.HasIndex(x => new { x.AssignmentId, x.EnrollmentId })
                .IsUnique();
        }
    }
}