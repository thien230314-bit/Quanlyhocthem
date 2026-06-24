using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjectById
{
    // Query lấy chi tiết một môn học
    public class GetSubjectByIdQuery : IRequest<Result<SubjectDetailDto>>
    {
        public Guid SubjectId { get; set; }
    }
}