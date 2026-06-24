using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Commands.UpdateUser
{
    // Command để Admin cập nhật thông tin tài khoản
    public class UpdateUserCommand : IRequest<Result>
    {
        // Id tài khoản cần cập nhật
        public Guid UserId { get; set; }

        // Họ tên
        public string FullName { get; set; } = string.Empty;

        // Email
        public string Email { get; set; } = string.Empty;

        // Số điện thoại
        public string? PhoneNumber { get; set; }
    }
}