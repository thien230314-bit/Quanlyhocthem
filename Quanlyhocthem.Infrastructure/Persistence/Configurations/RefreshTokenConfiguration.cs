using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng RefreshTokens
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Chuỗi refresh token
            builder.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired();

            // Thời gian hết hạn
            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            // Token thay thế
            builder.Property(x => x.ReplacedByToken)
                .HasMaxLength(500);

            // Một user có thể có nhiều refresh token
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mỗi refresh token phải là duy nhất
            builder.HasIndex(x => x.Token)
                .IsUnique();

            // Index để tìm refresh token theo user
            builder.HasIndex(x => x.UserId);

            // Index để kiểm tra token hết hạn
            builder.HasIndex(x => x.ExpiresAt);
        }
    }
}