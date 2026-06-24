using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Subjects
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tên môn học bắt buộc
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            // Mã môn học
            builder.Property(x => x.Code)
                .HasMaxLength(50);

            // Không cho trùng mã môn nếu có nhập
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[Code] IS NOT NULL");

            // Mô tả môn học
            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            // Trạng thái hoạt động
            builder.Property(x => x.IsActive)
                .IsRequired();
        }
    }
}