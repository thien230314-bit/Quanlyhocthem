using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassrooms
{
    // Handler lấy danh sách phòng học
    public class GetClassroomsQueryHandler : IRequestHandler<GetClassroomsQuery, Result<List<ClassroomDto>>>
    {
        private readonly IAppDbContext _context;

        public GetClassroomsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ClassroomDto>>> Handle(GetClassroomsQuery request, CancellationToken cancellationToken)
        {
            // Lấy toàn bộ phòng học, sắp xếp theo tên
            var classrooms = await _context.Classrooms
                .OrderBy(x => x.Name)
                .Select(x => new ClassroomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Capacity = x.Capacity,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<ClassroomDto>>.Success(classrooms);
        }
    }
}