using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.UpdateClassroom
{
    // Command để Admin cập nhật phòng học
    public class UpdateClassroomCommand : IRequest<Result>
    {
        // Id phòng học cần cập nhật
        public Guid ClassroomId { get; set; }

        // Tên phòng học
        public string Name { get; set; } = string.Empty;

        // Mã phòng học
        public string? Code { get; set; }

        // Sức chứa phòng học
        public int Capacity { get; set; }
    }
}