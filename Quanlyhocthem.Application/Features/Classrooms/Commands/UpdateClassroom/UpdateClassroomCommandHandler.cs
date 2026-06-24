using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.UpdateClassroom
{
    // Handler xử lý cập nhật phòng học
    public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateClassroomCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
        {
            if (request.ClassroomId == Guid.Empty)
                return Result.Failure("Id phòng học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result.Failure("Tên phòng học không được để trống.");

            if (request.Capacity < 1)
                return Result.Failure("Sức chứa phòng học phải lớn hơn 0.");

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(x => x.Id == request.ClassroomId, cancellationToken);

            if (classroom == null)
                return Result.Failure("Không tìm thấy phòng học.");

            var name = request.Name.Trim();
            var code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim();

            // Không cho trùng tên phòng với phòng khác
            var nameExists = await _context.Classrooms
                .AnyAsync(x =>
                    x.Id != request.ClassroomId &&
                    x.Name == name,
                    cancellationToken);

            if (nameExists)
                return Result.Failure("Tên phòng học đã tồn tại.");

            // Không cho trùng mã phòng với phòng khác
            if (!string.IsNullOrWhiteSpace(code))
            {
                var codeExists = await _context.Classrooms
                    .AnyAsync(x =>
                        x.Id != request.ClassroomId &&
                        x.Code == code,
                        cancellationToken);

                if (codeExists)
                    return Result.Failure("Mã phòng học đã tồn tại.");
            }

            classroom.Update(name, code, request.Capacity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}