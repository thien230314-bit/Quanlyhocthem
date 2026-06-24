using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.UpdateSubject
{
    // Command để Admin cập nhật môn học
    public class UpdateSubjectCommand : IRequest<Result>
    {
        // Id môn học cần cập nhật
        public Guid SubjectId { get; set; }

        // Tên môn học
        public string Name { get; set; } = string.Empty;

        // Mã môn học
        public string? Code { get; set; }

        // Mô tả môn học
        public string? Description { get; set; }
    }
}