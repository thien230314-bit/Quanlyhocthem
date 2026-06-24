using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetUserById
{
    // DTO chi tiết tài khoản
    public class UserDetailDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public Role Role { get; set; }

        public bool IsActive { get; set; }

        public Guid? StudentId { get; set; }

        public string? StudentCode { get; set; }

        public Guid? BatchId { get; set; }

        public string? BatchName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}