using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Commands.DeleteUser
{
    // Command để Admin xóa tài khoản
    public class DeleteUserCommand : IRequest<Result>
    {
        // Id tài khoản cần xóa
        public Guid UserId { get; set; }
    }
}