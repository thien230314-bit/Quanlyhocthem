namespace Quanlyhocthem.Application.Features.Subjects.Queries.GetSubjects
{
    // DTO trả danh sách môn học ra API
    public class SubjectDto
    {
        // Id môn học
        public Guid Id { get; set; }

        // Tên môn học
        public string Name { get; set; } = string.Empty;

        // Mã môn học
        public string? Code { get; set; }

        // Mô tả môn học
        public string? Description { get; set; }

        // Trạng thái hoạt động
        public bool IsActive { get; set; }
    }
}