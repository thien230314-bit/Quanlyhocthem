using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassroomById
{
    // Query lấy chi tiết một phòng học
    public class GetClassroomByIdQuery : IRequest<Result<ClassroomDetailDto>>
    {
        public Guid ClassroomId { get; set; }
    }
}