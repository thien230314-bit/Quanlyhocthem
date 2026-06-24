using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Commands.DeleteBatch
{
    // Command để Admin xóa khóa học viên
    public class DeleteBatchCommand : IRequest<Result>
    {
        // Id khóa học viên cần xóa
        public Guid BatchId { get; set; }
    }
}