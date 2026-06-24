using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Classrooms.Queries.GetClassrooms
{
    // Query lấy danh sách phòng học
    public class GetClassroomsQuery : IRequest<Result<List<ClassroomDto>>>
    {
    }
}