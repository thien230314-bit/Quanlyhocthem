using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Commands.UpdateCourse
{
    // Command để Admin cập nhật lớp học
    public class UpdateCourseCommand : IRequest<Result>
    {
        // Id lớp học cần cập nhật
        public Guid CourseId { get; set; }

        // Tên lớp học
        public string Name { get; set; } = string.Empty;

        // Mô tả lớp học
        public string Description { get; set; } = string.Empty;

        // Học phí
        public decimal TuitionFee { get; set; }

        // Sĩ số tối đa
        public int MaxStudents { get; set; }

        // Id môn học
        public Guid SubjectId { get; set; }

        // Id khóa học viên
        public Guid BatchId { get; set; }

        // Id phòng học, có thể null
        public Guid? ClassroomId { get; set; }

        // Id giảng viên, có thể null
        public Guid? TeacherId { get; set; }

        // Ngày bắt đầu
        public DateTime StartDate { get; set; }

        // Ngày kết thúc, có thể null
        public DateTime? EndDate { get; set; }
    }
}