using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Auth.Commands.Logout
{
    // Handler xử lý đăng xuất
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IAppDbContext _context;

        public LogoutCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result.Failure("Refresh token không được để trống.");

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (refreshToken == null)
                return Result.Failure("Refresh token không hợp lệ.");

            if (refreshToken.IsRevoked)
                return Result.Failure("Refresh token đã bị thu hồi trước đó.");

            refreshToken.Revoke(DateTime.UtcNow);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}