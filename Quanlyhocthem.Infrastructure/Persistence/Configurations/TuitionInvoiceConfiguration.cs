using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Infrastructure.Persistence.Configurations
{
    // Cấu hình bảng TuitionInvoices
    public class TuitionInvoiceConfiguration : IEntityTypeConfiguration<TuitionInvoice>
    {
        public void Configure(EntityTypeBuilder<TuitionInvoice> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);

            // Số tiền học phí
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Trạng thái hóa đơn: Unpaid, Paid, Overdue, Cancelled
            builder.Property(x => x.Status)
                .IsRequired();

            // Một hóa đơn thuộc về một Student
            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Một hóa đơn thuộc về một Course
            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}