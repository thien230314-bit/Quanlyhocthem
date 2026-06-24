using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Commands.UpdateBatch
{
    // Handler xử lý cập nhật khóa học viên
    public class UpdateBatchCommandHandler : IRequestHandler<UpdateBatchCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateBatchCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
        {
            if (request.BatchId == Guid.Empty)
                return Result.Failure("Id khóa học viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result.Failure("Tên khóa học viên không được để trống.");

            var batch = await _context.Batches
                .FirstOrDefaultAsync(x => x.Id == request.BatchId, cancellationToken);

            if (batch == null)
                return Result.Failure("Không tìm thấy khóa học viên.");

            var name = request.Name.Trim();

            // Không cho trùng tên với khóa khác
            var nameExists = await _context.Batches
                .AnyAsync(x =>
                    x.Id != request.BatchId &&
                    x.Name == name,
                    cancellationToken);

            if (nameExists)
                return Result.Failure("Tên khóa học viên đã tồn tại.");

            batch.Update(name, request.Description);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}