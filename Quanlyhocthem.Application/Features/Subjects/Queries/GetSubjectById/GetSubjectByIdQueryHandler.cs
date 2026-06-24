using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjectById
{
    // Handler lấy chi tiết môn học
    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, Result<SubjectDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetSubjectByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<SubjectDetailDto>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.SubjectId == Guid.Empty)
                return Result<SubjectDetailDto>.Failure("Id môn học không hợp lệ.");

            var subject = await _context.Subjects
                .Where(x => x.Id == request.SubjectId)
                .Select(x => new SubjectDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                return Result<SubjectDetailDto>.Failure("Không tìm thấy môn học.");

            return Result<SubjectDetailDto>.Success(subject);
        }
    }
}