using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Commands.CreateClassroom
{
    // Command dùng để Admin tạo phòng học
    public class CreateClassroomCommand : IRequest<Result<Guid>>
    {
        // Tên phòng học, ví dụ: Phòng A1
        public string Name { get; set; } = string.Empty;

        // Mã phòng học, ví dụ: A101
        public string? Code { get; set; }

        // Sức chứa tối đa của phòng
        public int Capacity { get; set; }
    }
}