using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Notifications
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Tiêu đề thông báo
            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            // Nội dung thông báo
            builder.Property(x => x.Message)
                .HasMaxLength(2000)
                .IsRequired();

            // Loại thông báo
            builder.Property(x => x.Type)
                .IsRequired();

            // Dữ liệu liên quan
            builder.Property(x => x.RelatedEntityType)
                .HasMaxLength(100);

            // Trạng thái đã đọc
            builder.Property(x => x.IsRead)
                .IsRequired();

            // Một User có nhiều Notification
            builder.HasOne(x => x.ReceiverUser)
                .WithMany()
                .HasForeignKey(x => x.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index để lấy thông báo của người dùng nhanh hơn
            builder.HasIndex(x => x.ReceiverUserId);

            // Index để lọc thông báo chưa đọc
            builder.HasIndex(x => new { x.ReceiverUserId, x.IsRead });
        }
    }
}