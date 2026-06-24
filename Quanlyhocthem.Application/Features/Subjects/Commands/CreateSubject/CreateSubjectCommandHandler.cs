using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.CreateSubject
{
    // Handler xử lý tạo môn học
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateSubjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            // Không cho tạo môn học trống tên
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<Guid>.Failure("Tên môn học không được để trống.");

            // Không cho trùng tên môn học
            var nameExists = await _context.Subjects
                .AnyAsync(x => x.Name == request.Name.Trim(), cancellationToken);

            if (nameExists)
                return Result<Guid>.Failure("Tên môn học đã tồn tại.");

            // Nếu có nhập mã môn thì không cho trùng mã
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var codeExists = await _context.Subjects
                    .AnyAsync(x => x.Code == request.Code.Trim(), cancellationToken);

                if (codeExists)
                    return Result<Guid>.Failure("Mã môn học đã tồn tại.");
            }

            // Tạo môn học mới
            var subject = new Subject(
                request.Name,
                request.Code,
                request.Description
            );

            _context.Subjects.Add(subject);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(subject.Id);
        }
    }
}