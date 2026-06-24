using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Features.Batches.Commands.CreateBatch
{
    // Handler xử lý tạo khóa học viên
    public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateBatchCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
        {
            // Không cho tạo khóa trống tên
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<Guid>.Failure("Tên khóa học viên không được để trống.");

            // Không cho trùng tên khóa
            var exists = await _context.Batches
                .AnyAsync(x => x.Name == request.Name.Trim(), cancellationToken);

            if (exists)
                return Result<Guid>.Failure("Tên khóa học viên đã tồn tại.");

            // Tạo khóa học viên mới
            var batch = new Batch(request.Name, request.Description);

            _context.Batches.Add(batch);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(batch.Id);
        }
    }
}