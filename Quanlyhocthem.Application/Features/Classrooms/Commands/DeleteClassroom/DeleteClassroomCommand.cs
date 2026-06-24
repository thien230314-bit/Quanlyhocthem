using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.DeleteClassroom
{
    // Command để Admin xóa phòng học
    public class DeleteClassroomCommand : IRequest<Result>
    {
        // Id phòng học cần xóa
        public Guid ClassroomId { get; set; }
    }
}