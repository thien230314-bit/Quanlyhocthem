using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.DeleteSubject
{
    // Command để Admin xóa môn học
    public class DeleteSubjectCommand : IRequest<Result>
    {
        // Id môn học cần xóa
        public Guid SubjectId { get; set; }
    }
}