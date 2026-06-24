using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Assignment là bài tập do giảng viên tạo cho một lớp học
    public class Assignment : BaseEntity
    {
        // Lớp học được giao bài tập
        public Guid CourseId { get; private set; }

        public Course Course { get; private set; } = null!;

        // Giảng viên tạo bài tập
        public Guid TeacherId { get; private set; }

        public User Teacher { get; private set; } = null!;

        // Tiêu đề bài tập
        public string Title { get; private set; } = string.Empty;

        // Nội dung / mô tả bài tập
        public string Description { get; private set; } = string.Empty;

        // Hạn nộp bài
        public DateTime DueDate { get; private set; }

        // Tên file đính kèm nếu có
        public string? AttachmentFileName { get; private set; }

        // Đường dẫn file đính kèm nếu có
        public string? AttachmentUrl { get; private set; }

        // Điểm tối đa của bài tập
        public decimal MaxScore { get; private set; }

        // Bài tập còn hoạt động không
        public bool IsActive { get; private set; } = true;

        // Danh sách bài nộp của học viên
        public ICollection<Submission> Submissions { get; private set; } = new List<Submission>();

        private Assignment()
        {
        }

        public Assignment(
            Guid courseId,
            Guid teacherId,
            string title,
            string description,
            DateTime dueDate,
            decimal maxScore,
            string? attachmentFileName,
            string? attachmentUrl)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Tiêu đề bài tập không được để trống.");

            if (dueDate <= DateTime.UtcNow)
                throw new ArgumentException("Hạn nộp bài phải lớn hơn thời gian hiện tại.");

            if (maxScore <= 0)
                throw new ArgumentException("Điểm tối đa phải lớn hơn 0.");

            CourseId = courseId;
            TeacherId = teacherId;
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            DueDate = dueDate;
            MaxScore = maxScore;
            AttachmentFileName = attachmentFileName?.Trim();
            AttachmentUrl = attachmentUrl?.Trim();
            IsActive = true;
        }

        public void Update(
            string title,
            string description,
            DateTime dueDate,
            decimal maxScore,
            string? attachmentFileName,
            string? attachmentUrl)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Tiêu đề bài tập không được để trống.");

            if (maxScore <= 0)
                throw new ArgumentException("Điểm tối đa phải lớn hơn 0.");

            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            DueDate = dueDate;
            MaxScore = maxScore;
            AttachmentFileName = attachmentFileName?.Trim();
            AttachmentUrl = attachmentUrl?.Trim();
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}