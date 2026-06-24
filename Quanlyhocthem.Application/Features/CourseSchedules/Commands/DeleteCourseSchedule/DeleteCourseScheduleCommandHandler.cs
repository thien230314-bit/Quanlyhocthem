using MediatR;
using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseSchedules.Commands.DeleteCourseSchedule
{
    // Handler xử lý Admin xóa lịch học
    public class DeleteCourseScheduleCommandHandler : IRequestHandler<DeleteCourseScheduleCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteCourseScheduleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCourseScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseScheduleId == Guid.Empty)
                return Result.Failure("Id lịch học không hợp lệ.");

            var schedule = await _context.CourseSchedules
                .FirstOrDefaultAsync(x => x.Id == request.CourseScheduleId, cancellationToken);

            if (schedule == null)
                return Result.Failure("Không tìm thấy lịch học.");

            _context.CourseSchedules.Remove(schedule);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}