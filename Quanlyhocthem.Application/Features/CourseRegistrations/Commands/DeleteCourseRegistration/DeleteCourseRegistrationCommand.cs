using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.CourseRegistrations.Commands.DeleteCourseRegistration
{
    // Command để học viên hủy đăng ký lớp học
    public class DeleteCourseRegistrationCommand : IRequest<Result>
    {
        // Id User của học viên, lấy từ JWT token
        public Guid StudentUserId { get; set; }

        // Id lớp học cần hủy đăng ký
        public Guid CourseId { get; set; }
    }
}