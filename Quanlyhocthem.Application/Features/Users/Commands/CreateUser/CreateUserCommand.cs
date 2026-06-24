using MediatR;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Users.Commands.CreateUser
{
    // Command dùng để Admin tạo tài khoản
    public class CreateUserCommand : IRequest<Result<Guid>>
    {
        // Tên đăng nhập
        public string UserName { get; set; } = string.Empty;

        // Mật khẩu
        public string Password { get; set; } = string.Empty;

        // Họ tên
        public string FullName { get; set; } = string.Empty;

        // Email
        public string Email { get; set; } = string.Empty;

        // Số điện thoại
        public string? PhoneNumber { get; set; }

        // Vai trò: Admin, Teacher, Student
        public Role Role { get; set; }

        // Mã học viên, chỉ bắt buộc nếu Role = Student
        public string? StudentCode { get; set; }

        // Ngày sinh, chỉ bắt buộc nếu Role = Student
        public DateTime? DateOfBirth { get; set; }

        // Khóa của học viên, chỉ bắt buộc nếu Role = Student
        public Guid? BatchId { get; set; }
    }
}