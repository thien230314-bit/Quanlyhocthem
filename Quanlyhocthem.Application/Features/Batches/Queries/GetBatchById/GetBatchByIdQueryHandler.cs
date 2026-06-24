using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Queries.GetBatchById
{
    // Handler lấy chi tiết khóa học viên
    public class GetBatchByIdQueryHandler : IRequestHandler<GetBatchByIdQuery, Result<BatchDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetBatchByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<BatchDetailDto>> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.BatchId == Guid.Empty)
                return Result<BatchDetailDto>.Failure("Id khóa học viên không hợp lệ.");

            var batch = await _context.Batches
                .Where(x => x.Id == request.BatchId)
                .Select(x => new BatchDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    TotalStudents = _context.Students.Count(s => s.BatchId == x.Id),
                    TotalCourses = _context.Courses.Count(c => c.BatchId == x.Id),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (batch == null)
                return Result<BatchDetailDto>.Failure("Không tìm thấy khóa học viên.");

            return Result<BatchDetailDto>.Success(batch);
        }
    }
}