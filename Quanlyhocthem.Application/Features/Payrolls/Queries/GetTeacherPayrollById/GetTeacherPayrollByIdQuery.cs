using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Payrolls.Queries.GetTeacherPayrollById
{
    // Query để giảng viên xem chi tiết một bảng lương của mình
    public class GetTeacherPayrollByIdQuery : IRequest<Result<TeacherPayrollDetailDto>>
    {
        // Id User của giảng viên, lấy từ JWT token
        public Guid TeacherUserId { get; set; }

        // Id bảng lương
        public Guid PayrollId { get; set; }
    }
}