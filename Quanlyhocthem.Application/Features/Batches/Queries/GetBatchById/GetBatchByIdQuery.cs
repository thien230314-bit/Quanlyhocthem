using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatchById
{
    // Query lấy chi tiết một khóa học viên
    public class GetBatchByIdQuery : IRequest<Result<BatchDetailDto>>
    {
        public Guid BatchId { get; set; }
    }
}