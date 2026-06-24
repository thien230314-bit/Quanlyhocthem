using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetStudents
{
    // Handler lấy danh sách học sinh cho Admin
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, Result<List<StudentDto>>>
    {
        private readonly IAppDbContext _context;

        public GetStudentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            // Lấy Student kèm User và Batch
            var students = await _context.Students
                .Include(x => x.User)
                .Include(x => x.Batch)
                .OrderBy(x => x.User.FullName)
                .Select(x => new StudentDto
                {
                    UserId = x.UserId,
                    StudentId = x.Id,
                    StudentCode = x.StudentCode,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    DateOfBirth = x.DateOfBirth,
                    BatchId = x.BatchId,
                    BatchName = x.Batch.Name,
                    IsActive = x.User.IsActive,
                    CreatedAt = x.User.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<StudentDto>>.Success(students);
        }
    }
}