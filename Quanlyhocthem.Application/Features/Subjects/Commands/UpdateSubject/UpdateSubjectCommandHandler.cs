using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.UpdateSubject
{
    // Handler xử lý cập nhật môn học
    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateSubjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            if (request.SubjectId == Guid.Empty)
                return Result.Failure("Id môn học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result.Failure("Tên môn học không được để trống.");

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == request.SubjectId, cancellationToken);

            if (subject == null)
                return Result.Failure("Không tìm thấy môn học.");

            var name = request.Name.Trim();
            var code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim();

            // Không cho trùng tên với môn học khác
            var nameExists = await _context.Subjects
                .AnyAsync(x =>
                    x.Id != request.SubjectId &&
                    x.Name == name,
                    cancellationToken);

            if (nameExists)
                return Result.Failure("Tên môn học đã tồn tại.");

            // Không cho trùng mã với môn học khác
            if (!string.IsNullOrWhiteSpace(code))
            {
                var codeExists = await _context.Subjects
                    .AnyAsync(x =>
                        x.Id != request.SubjectId &&
                        x.Code == code,
                        cancellationToken);

                if (codeExists)
                    return Result.Failure("Mã môn học đã tồn tại.");
            }

            subject.Update(name, code, request.Description);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}