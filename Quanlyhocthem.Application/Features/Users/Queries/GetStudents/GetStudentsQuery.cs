using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetStudents
{
    // Query lấy danh sách học sinh
    public class GetStudentsQuery : IRequest<Result<List<StudentDto>>>
    {
    }
}