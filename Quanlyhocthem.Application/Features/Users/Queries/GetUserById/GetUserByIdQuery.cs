using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetUserById
{
    // Query lấy chi tiết một tài khoản
    public class GetUserByIdQuery : IRequest<Result<UserDetailDto>>
    {
        public Guid UserId { get; set; }
    }
}