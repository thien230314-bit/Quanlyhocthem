using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng Users trong SQL Server
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // UserName bắt buộc và không được trùng
            builder.Property(x => x.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.UserName)
                .IsUnique();

            // Mật khẩu bắt buộc
            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            // Họ tên bắt buộc
            builder.Property(x => x.FullName)
                .HasMaxLength(200)
                .IsRequired();

            // Email bắt buộc và không được trùng
            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            // Số điện thoại không bắt buộc
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            // Role chỉ có Admin, Teacher, Student
            builder.Property(x => x.Role)
                .IsRequired();

            // IsActive dùng để khóa/mở tài khoản
            builder.Property(x => x.IsActive)
                .IsRequired();

            // RefreshToken dùng khi làm JWT refresh token
            builder.Property(x => x.RefreshToken)
                .HasMaxLength(1000);
        }
    }
}
