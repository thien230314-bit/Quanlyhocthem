using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassroomById
{
    // Handler lấy chi tiết phòng học
    public class GetClassroomByIdQueryHandler : IRequestHandler<GetClassroomByIdQuery, Result<ClassroomDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetClassroomByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ClassroomDetailDto>> Handle(GetClassroomByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.ClassroomId == Guid.Empty)
                return Result<ClassroomDetailDto>.Failure("Id phòng học không hợp lệ.");

            var classroom = await _context.Classrooms
                .Where(x => x.Id == request.ClassroomId)
                .Select(x => new ClassroomDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Capacity = x.Capacity,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (classroom == null)
                return Result<ClassroomDetailDto>.Failure("Không tìm thấy phòng học.");

            return Result<ClassroomDetailDto>.Success(classroom);
        }
    }
}