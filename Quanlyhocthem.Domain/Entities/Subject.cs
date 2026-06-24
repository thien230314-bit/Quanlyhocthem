using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Subject là môn học, ví dụ: Toán, Lý, Hóa, Tiếng Anh
    public class Subject : BaseEntity
    {
        // Tên môn học
        public string Name { get; private set; } = string.Empty;

        // Mã môn học, ví dụ: MATH12
        public string? Code { get; private set; }

        // Mô tả môn học
        public string? Description { get; private set; }

        // Môn học còn hoạt động không
        public bool IsActive { get; private set; } = true;

        // Danh sách lớp thuộc môn này
        public ICollection<Course> Courses { get; private set; } = new List<Course>();

        private Subject()
        {
        }

        public Subject(string name, string? code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên môn học không được để trống.");

            Name = name.Trim();
            Code = code?.Trim();
            Description = description?.Trim();
            IsActive = true;
        }

        public void Update(string name, string? code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên môn học không được để trống.");

            Name = name.Trim();
            Code = code?.Trim();
            Description = description?.Trim();
            SetUpdatedAt();
        }
    }
}