using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Queries.GetCourseById
{
    // Query lấy chi tiết một lớp học
    public class GetCourseByIdQuery : IRequest<Result<CourseDetailDto>>
    {
        public Guid CourseId { get; set; }
    }
}