using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.DeleteClassroom
{
    // Handler xử lý xóa phòng học
    public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteClassroomCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
        {
            if (request.ClassroomId == Guid.Empty)
                return Result.Failure("Id phòng học không hợp lệ.");

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(x => x.Id == request.ClassroomId, cancellationToken);

            if (classroom == null)
                return Result.Failure("Không tìm thấy phòng học.");

            // Nếu phòng đã được lớp học sử dụng thì không cho xóa
            var hasCourses = await _context.Courses
                .AnyAsync(x => x.ClassroomId == request.ClassroomId, cancellationToken);

            if (hasCourses)
                return Result.Failure("Không thể xóa phòng học vì đã có lớp học sử dụng phòng này.");

            _context.Classrooms.Remove(classroom);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}