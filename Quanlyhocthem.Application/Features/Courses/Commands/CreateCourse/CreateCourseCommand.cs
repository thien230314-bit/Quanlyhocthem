using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Courses.Commands.CreateCourse
{
    // Command dùng để Admin tạo lớp học cụ thể
    // Course ở đây là lớp học đã gắn môn học, khóa, phòng và giảng viên
    public class CreateCourseCommand : IRequest<Result<Guid>>
    {
        // Tên lớp học, ví dụ: Toán 12 - Ca tối
        public string Name { get; set; } = string.Empty;

        // Mô tả lớp học
        public string Description { get; set; } = string.Empty;

        // Học phí của lớp
        public decimal TuitionFee { get; set; }

        // Sĩ số tối đa của lớp
        public int MaxStudents { get; set; }

        // Id môn học, ví dụ: Toán, Lý, Hóa
        public Guid SubjectId { get; set; }

        // Id khóa học viên, ví dụ: Khóa 2025
        public Guid BatchId { get; set; }

        // Id phòng học, có thể null nếu chưa gán phòng
        public Guid? ClassroomId { get; set; }

        // Id giảng viên được gán cho lớp
        public Guid? TeacherId { get; set; }

        // Ngày bắt đầu lớp học
        public DateTime StartDate { get; set; }

        // Ngày kết thúc lớp học, có thể null
        public DateTime? EndDate { get; set; }
    }
}