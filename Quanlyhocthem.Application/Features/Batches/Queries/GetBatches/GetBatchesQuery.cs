using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatches
{
    // Query lấy danh sách khóa học viên
    public class GetBatchesQuery : IRequest<Result<List<BatchDto>>>
    {
    }
}