using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.DeleteSubject
{
    // Handler xử lý xóa môn học
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteSubjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            if (request.SubjectId == Guid.Empty)
                return Result.Failure("Id môn học không hợp lệ.");

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == request.SubjectId, cancellationToken);

            if (subject == null)
                return Result.Failure("Không tìm thấy môn học.");

            // Nếu môn học đã được dùng trong lớp học thì không cho xóa
            var hasCourses = await _context.Courses
                .AnyAsync(x => x.SubjectId == request.SubjectId, cancellationToken);

            if (hasCourses)
                return Result.Failure("Không thể xóa môn học vì đã có lớp học sử dụng môn này.");

            _context.Subjects.Remove(subject);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}