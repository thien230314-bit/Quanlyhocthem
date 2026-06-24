using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatches
{
    // Handler lấy danh sách khóa học viên
    public class GetBatchesQueryHandler : IRequestHandler<GetBatchesQuery, Result<List<BatchDto>>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<BatchDto>>> Handle(GetBatchesQuery request, CancellationToken cancellationToken)
        {
            // Lấy toàn bộ khóa học viên, sắp xếp theo tên
            var batches = await _context.Batches
                .OrderBy(x => x.Name)
                .Select(x => new BatchDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<BatchDto>>.Success(batches);
        }
    }
}