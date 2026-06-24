using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Commands.DeleteAssignment
{
    // Command để giảng viên xóa bài tập đã giao
    public class DeleteAssignmentCommand : IRequest<Result>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bài tập cần xóa
        public Guid AssignmentId { get; set; }
    }
}