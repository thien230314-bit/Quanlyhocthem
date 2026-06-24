using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;

namespace Quanlyhocthem.Application.Features.Users.Commands.CreateUser
{
    // Handler xử lý tạo tài khoản người dùng
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Chỉ cho phép 3 role đúng theo giao diện
            if (!Enum.IsDefined(typeof(Role), request.Role))
                return Result<Guid>.Failure("Vai trò người dùng không hợp lệ.");

            // Kiểm tra trùng tên đăng nhập
            var userNameExists = await _context.Users
                .AnyAsync(x => x.UserName == request.UserName.Trim(), cancellationToken);

            if (userNameExists)
                return Result<Guid>.Failure("Tên đăng nhập đã tồn tại.");

            // Kiểm tra trùng email
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == request.Email.Trim(), cancellationToken);

            if (emailExists)
                return Result<Guid>.Failure("Email đã tồn tại.");

            // Tạo tài khoản user
            var user = new User(
                request.UserName,
                request.Password,
                request.FullName,
                request.Email,
                request.PhoneNumber,
                request.Role
            );

            _context.Users.Add(user);

            // Nếu role là Student thì tạo thêm hồ sơ học viên
            if (request.Role == Role.Student)
            {
                // StudentCode bắt buộc cho học viên
                if (string.IsNullOrWhiteSpace(request.StudentCode))
                    return Result<Guid>.Failure("Mã học viên không được để trống.");

                // DateOfBirth bắt buộc cho học viên
                if (!request.DateOfBirth.HasValue)
                    return Result<Guid>.Failure("Ngày sinh học viên không được để trống.");

                // BatchId bắt buộc để biết học viên thuộc khóa nào
                if (!request.BatchId.HasValue)
                    return Result<Guid>.Failure("Khóa học viên không được để trống.");

                // Kiểm tra Batch có tồn tại không
                var batchExists = await _context.Batches
                    .AnyAsync(x => x.Id == request.BatchId.Value, cancellationToken);

                if (!batchExists)
                    return Result<Guid>.Failure("Khóa học viên không tồn tại.");

                // Không cho trùng mã học viên
                var studentCodeExists = await _context.Students
                    .AnyAsync(x => x.StudentCode == request.StudentCode.Trim(), cancellationToken);

                if (studentCodeExists)
                    return Result<Guid>.Failure("Mã học viên đã tồn tại.");

                // Tạo hồ sơ học viên, không có ParentName/ParentPhone
                var student = new Student(
                    request.StudentCode,
                    request.DateOfBirth.Value,
                    user.Id,
                    request.BatchId.Value
                );

                _context.Students.Add(student);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}