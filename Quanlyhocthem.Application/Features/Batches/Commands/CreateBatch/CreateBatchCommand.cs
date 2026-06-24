using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Commands.CreateBatch
{
    // Command dùng để Admin tạo khóa học viên
    public class CreateBatchCommand : IRequest<Result<Guid>>
    {
        // Tên khóa, ví dụ: Khóa 2025
        public string Name { get; set; } = string.Empty;

        // Mô tả khóa học viên
        public string? Description { get; set; }
    }
}