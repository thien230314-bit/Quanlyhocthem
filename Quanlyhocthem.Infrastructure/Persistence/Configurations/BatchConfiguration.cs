using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Batches
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tên khóa bắt buộc
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            // Mô tả khóa
            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            // Trạng thái hoạt động
            builder.Property(x => x.IsActive)
                .IsRequired();
        }
    }
}