using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using RefreshTokenEntity = Quanlyhocthem.Domain.Entities.RefreshToken;

namespace Quanlyhocthem.Application.Features.Auth.Commands.Login
{
    // Handler xử lý đăng nhập
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResultDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(IAppDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return Result<LoginResultDto>.Failure("Tên đăng nhập không được để trống.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result<LoginResultDto>.Failure("Mật khẩu không được để trống.");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == request.UserName.Trim(), cancellationToken);

            if (user == null)
                return Result<LoginResultDto>.Failure("Tên đăng nhập hoặc mật khẩu không đúng.");

            if (!user.IsActive)
                return Result<LoginResultDto>.Failure("Tài khoản bạn đã bị khóa, liên hệ admin.");

            // Hiện tại project đang so sánh trực tiếp PasswordHash theo dữ liệu đang lưu.
            // Nếu sau này dùng BCrypt thì thay đoạn này bằng BCrypt.Verify.
            var passwordValid = IsPasswordValid(request.Password, user.PasswordHash);

            if (!passwordValid)
                return Result<LoginResultDto>.Failure("Tên đăng nhập hoặc mật khẩu không đúng.");

            // Tạo Access Token kèm thời gian hết hạn
            var accessTokenResult = _jwtTokenService.GenerateAccessToken(user);

            // Tạo Refresh Token kèm thời gian hết hạn
            var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenExpiresAt = _jwtTokenService.GetRefreshTokenExpiresAt();

            var refreshToken = new RefreshTokenEntity(
                user.Id,
                refreshTokenValue,
                refreshTokenExpiresAt
            );

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            var result = new LoginResultDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessTokenResult.AccessToken,
                RefreshToken = refreshTokenValue,
                AccessTokenExpiresAt = accessTokenResult.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };

            return Result<LoginResultDto>.Success(result);
        }

        // Kiểm tra mật khẩu
        private static bool IsPasswordValid(string inputPassword, string storedPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(inputPassword))
                return false;

            if (string.IsNullOrWhiteSpace(storedPasswordHash))
                return false;

            return inputPassword == storedPasswordHash;
        }
    }
}