using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Commands.DeleteCourse
{
    // Command để Admin xóa lớp học
    public class DeleteCourseCommand : IRequest<Result>
    {
        // Id lớp học cần xóa
        public Guid CourseId { get; set; }
    }
}