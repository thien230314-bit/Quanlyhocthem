using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Commands.UpdateBatch
{
    // Command để Admin cập nhật khóa học viên
    public class UpdateBatchCommand : IRequest<Result>
    {
        // Id khóa học viên cần cập nhật
        public Guid BatchId { get; set; }

        // Tên khóa học viên
        public string Name { get; set; } = string.Empty;

        // Mô tả khóa học viên
        public string? Description { get; set; }
    }
}