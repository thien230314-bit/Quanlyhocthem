using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;
using Quanlyhocthem.Domain.Entities;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.CreateClassroom
{
    // Handler xử lý tạo phòng học
    public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public CreateClassroomCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
        {
            // Không cho tạo phòng học trống tên
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<Guid>.Failure("Tên phòng học không được để trống.");

            // Sức chứa phòng phải hợp lệ
            if (request.Capacity < 1)
                return Result<Guid>.Failure("Sức chứa phòng học phải lớn hơn 0.");

            // Không cho trùng tên phòng
            var nameExists = await _context.Classrooms
                .AnyAsync(x => x.Name == request.Name.Trim(), cancellationToken);

            if (nameExists)
                return Result<Guid>.Failure("Tên phòng học đã tồn tại.");

            // Nếu có nhập mã phòng thì không cho trùng mã
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var codeExists = await _context.Classrooms
                    .AnyAsync(x => x.Code == request.Code.Trim(), cancellationToken);

                if (codeExists)
                    return Result<Guid>.Failure("Mã phòng học đã tồn tại.");
            }

            // Tạo phòng học mới
            var classroom = new Classroom(
                request.Name,
                request.Code,
                request.Capacity
            );

            _context.Classrooms.Add(classroom);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(classroom.Id);
        }
    }
}