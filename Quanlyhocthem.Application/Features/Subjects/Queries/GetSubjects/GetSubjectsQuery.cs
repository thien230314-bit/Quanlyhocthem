using MediatR;
using Quanlyhocthem.Application.Common.Models;

namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjects
{
    // Query lấy danh sách môn học
    public class GetSubjectsQuery : IRequest<Result<List<SubjectDto>>>
    {
    }
}