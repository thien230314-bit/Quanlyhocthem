using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Application.Features.Auth.Commands.Login;
using RefreshTokenEntity = Quanlyhocthem.Domain.Entities.RefreshToken;

namespace Quanlyhocthem.Application.Features.Auth.Commands.RefreshToken
{
    // Handler xử lý cấp lại Access Token bằng Refresh Token
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResultDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(IAppDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result<LoginResultDto>.Failure("Refresh token không được để trống.");

            // Tìm refresh token trong database
            var oldRefreshToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (oldRefreshToken == null)
                return Result<LoginResultDto>.Failure("Refresh token không hợp lệ.");

            if (oldRefreshToken.IsRevoked)
                return Result<LoginResultDto>.Failure("Refresh token đã bị thu hồi.");

            if (oldRefreshToken.IsExpired)
                return Result<LoginResultDto>.Failure("Refresh token đã hết hạn. Vui lòng đăng nhập lại.");

            var user = oldRefreshToken.User;

            if (!user.IsActive)
                return Result<LoginResultDto>.Failure("Tài khoản bạn đã bị khóa, liên hệ admin.");

            // Tạo Access Token mới
            var accessTokenResult = _jwtTokenService.GenerateAccessToken(user);

            // Tạo Refresh Token mới để thay thế token cũ
            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();
            var newRefreshTokenExpiresAt = _jwtTokenService.GetRefreshTokenExpiresAt();

            var newRefreshToken = new RefreshTokenEntity(
                user.Id,
                newRefreshTokenValue,
                newRefreshTokenExpiresAt
            );

            // Thu hồi Refresh Token cũ để tránh dùng lại
            oldRefreshToken.Revoke(DateTime.UtcNow, newRefreshTokenValue);

            _context.RefreshTokens.Add(newRefreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            var result = new LoginResultDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessTokenResult.AccessToken,
                RefreshToken = newRefreshTokenValue,
                AccessTokenExpiresAt = accessTokenResult.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = newRefreshTokenExpiresAt
            };

            return Result<LoginResultDto>.Success(result);
        }
    }
}