using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Batches.Commands.DeleteBatch
{
    // Handler xử lý xóa khóa học viên
    public class DeleteBatchCommandHandler : IRequestHandler<DeleteBatchCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteBatchCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            if (request.BatchId == Guid.Empty)
                return Result.Failure("Id khóa học viên không hợp lệ.");

            var batch = await _context.Batches
                .FirstOrDefaultAsync(x => x.Id == request.BatchId, cancellationToken);

            if (batch == null)
                return Result.Failure("Không tìm thấy khóa học viên.");

            // Nếu khóa đã có học viên thì không cho xóa
            var hasStudents = await _context.Students
                .AnyAsync(x => x.BatchId == request.BatchId, cancellationToken);

            if (hasStudents)
                return Result.Failure("Không thể xóa khóa học viên vì đã có học viên thuộc khóa này.");

            // Nếu khóa đã có lớp học thì không cho xóa
            var hasCourses = await _context.Courses
                .AnyAsync(x => x.BatchId == request.BatchId, cancellationToken);

            if (hasCourses)
                return Result.Failure("Không thể xóa khóa học viên vì đã có lớp học thuộc khóa này.");

            _context.Batches.Remove(batch);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}