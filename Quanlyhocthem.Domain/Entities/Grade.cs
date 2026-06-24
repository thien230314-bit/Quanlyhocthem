using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Grade là điểm và nhận xét của giảng viên cho một bài nộp
    public class Grade : BaseEntity
    {
        // Bài nộp được chấm
        public Guid SubmissionId { get; private set; }

        public Submission Submission { get; private set; } = null!;

        // Giảng viên chấm điểm
        public Guid TeacherId { get; private set; }

        public User Teacher { get; private set; } = null!;

        // Điểm số
        public decimal Score { get; private set; }

        // Nhận xét của giảng viên
        public string? Feedback { get; private set; }

        // Thời gian chấm
        public DateTime GradedAt { get; private set; }

        private Grade()
        {
        }

        public Grade(Guid submissionId, Guid teacherId, decimal score, string? feedback, DateTime gradedAt)
        {
            if (score < 0)
                throw new ArgumentException("Điểm không được nhỏ hơn 0.");

            SubmissionId = submissionId;
            TeacherId = teacherId;
            Score = score;
            Feedback = feedback?.Trim();
            GradedAt = gradedAt;
        }

        public void Update(decimal score, string? feedback, DateTime gradedAt)
        {
            if (score < 0)
                throw new ArgumentException("Điểm không được nhỏ hơn 0.");

            Score = score;
            Feedback = feedback?.Trim();
            GradedAt = gradedAt;
            SetUpdatedAt();
        }
    }
}