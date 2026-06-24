using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Commands.UpdateUser
{
    // Handler xử lý cập nhật tài khoản
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return Result.Failure("Id tài khoản không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.FullName))
                return Result.Failure("Họ tên không được để trống.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return Result.Failure("Email không được để trống.");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null)
                return Result.Failure("Không tìm thấy tài khoản.");

            var email = request.Email.Trim();

            // Không cho trùng email với tài khoản khác
            var emailExists = await _context.Users
                .AnyAsync(x =>
                    x.Id != request.UserId &&
                    x.Email == email,
                    cancellationToken);

            if (emailExists)
                return Result.Failure("Email đã được sử dụng bởi tài khoản khác.");

            user.UpdateProfile(request.FullName, email, request.PhoneNumber);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}