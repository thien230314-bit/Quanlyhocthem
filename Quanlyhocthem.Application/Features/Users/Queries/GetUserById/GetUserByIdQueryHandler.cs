using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetUserById
{
    // Handler lấy chi tiết tài khoản
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailDto>>
    {
        private readonly IAppDbContext _context;

        public GetUserByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return Result<UserDetailDto>.Failure("Id tài khoản không hợp lệ.");

            var user = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Select(x => new UserDetailDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Role = x.Role,
                    IsActive = x.IsActive,

                    StudentId = _context.Students
                        .Where(s => s.UserId == x.Id)
                        .Select(s => (Guid?)s.Id)
                        .FirstOrDefault(),

                    StudentCode = _context.Students
                        .Where(s => s.UserId == x.Id)
                        .Select(s => s.StudentCode)
                        .FirstOrDefault(),

                    BatchId = _context.Students
                        .Where(s => s.UserId == x.Id)
                        .Select(s => (Guid?)s.BatchId)
                        .FirstOrDefault(),

                    BatchName = _context.Students
                        .Where(s => s.UserId == x.Id)
                        .Select(s => s.Batch.Name)
                        .FirstOrDefault(),

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return Result<UserDetailDto>.Failure("Không tìm thấy tài khoản.");

            return Result<UserDetailDto>.Success(user);
        }
    }
}