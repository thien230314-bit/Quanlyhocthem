using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Commands.CreateSubject
{
    // Command dùng để Admin tạo môn học
    public class CreateSubjectCommand : IRequest<Result<Guid>>
    {
        // Tên môn học, ví dụ: Toán, Lý, Hóa, Tiếng Anh
        public string Name { get; set; } = string.Empty;

        // Mã môn học, ví dụ: MATH12, ENG01
        public string? Code { get; set; }

        // Mô tả môn học
        public string? Description { get; set; }
    }
}