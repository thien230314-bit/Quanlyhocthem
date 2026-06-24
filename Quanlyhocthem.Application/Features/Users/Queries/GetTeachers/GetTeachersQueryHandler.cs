using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetTeachers
{
    // Handler lấy danh sách giảng viên cho Admin
    public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, Result<List<TeacherDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTeachersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TeacherDto>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
        {
            // Lấy các User có Role = Teacher
            var teachers = await _context.Users
                .Where(x => x.Role == Role.Teacher)
                .OrderBy(x => x.FullName)
                .Select(x => new TeacherDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<TeacherDto>>.Success(teachers);
        }
    }
}