using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Batch là khóa học viên, dùng để nhóm học viên và lớp học
    public class Batch : BaseEntity
    {
        // Tên khóa, ví dụ: Khóa 2025, IELTS Foundation
        public string Name { get; private set; } = string.Empty;

        // Mô tả khóa
        public string? Description { get; private set; }

        // Khóa còn hoạt động không
        public bool IsActive { get; private set; } = true;

        // Danh sách lớp thuộc khóa này
        public ICollection<Course> Courses { get; private set; } = new List<Course>();

        private Batch()
        {
        }

        public Batch(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên khóa không được để trống.");

            Name = name.Trim();
            Description = description?.Trim();
            IsActive = true;
        }

        public void Update(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên khóa không được để trống.");

            Name = name.Trim();
            Description = description?.Trim();
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }
    }
}