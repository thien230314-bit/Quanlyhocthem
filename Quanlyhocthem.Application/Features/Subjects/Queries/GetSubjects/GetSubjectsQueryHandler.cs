using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjects
{
    // Handler lấy danh sách môn học
    public class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, Result<List<SubjectDto>>>
    {
        private readonly IAppDbContext _context;

        public GetSubjectsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<SubjectDto>>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            // Lấy toàn bộ môn học, sắp xếp theo tên
            var subjects = await _context.Subjects
                .OrderBy(x => x.Name)
                .Select(x => new SubjectDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<SubjectDto>>.Success(subjects);
        }
    }
}