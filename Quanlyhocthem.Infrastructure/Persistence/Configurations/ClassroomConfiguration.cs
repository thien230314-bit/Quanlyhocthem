using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Classrooms
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tên phòng bắt buộc
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            // Mã phòng
            builder.Property(x => x.Code)
                .HasMaxLength(50);

            // Không cho trùng mã phòng nếu có nhập
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[Code] IS NOT NULL");

            // Sức chứa phòng
            builder.Property(x => x.Capacity)
                .IsRequired();

            // Trạng thái phòng
            builder.Property(x => x.IsActive)
                .IsRequired();
        }
    }
}