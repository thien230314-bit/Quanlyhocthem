using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Users.Queries.GetTeachers
{
    // Query lấy danh sách giảng viên
    public class GetTeachersQuery : IRequest<Result<List<TeacherDto>>>
    {
    }
}