using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Commands.RegisterCourse
{
    // Command dùng để học viên đăng ký khóa học
    public class RegisterCourseCommand : IRequest<Result<Guid>>
    {
        // UserId của học viên hiện tại, lấy từ token trong Controller
        public Guid StudentUserId { get; set; }

        // Id khóa học muốn đăng ký
        public Guid CourseId { get; set; }
    }
}