using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Assignments.Queries.GetAssignmentById
{
    // Query lấy chi tiết một bài tập
    public class GetAssignmentByIdQuery : IRequest<Result<AssignmentDetailDto>>
    {
        // Id User hiện tại, lấy từ JWT token
        public Guid CurrentUserId { get; set; }

        // Id bài tập cần xem
        public Guid AssignmentId { get; set; }
    }
}