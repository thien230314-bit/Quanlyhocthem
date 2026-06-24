using Quanlyhocthem.Domain.Common;

namespace Quanlyhocthem.Domain.Entities
{
    // Classroom là phòng học
    public class Classroom : BaseEntity
    {
        // Tên phòng, ví dụ: Phòng A1
        public string Name { get; private set; } = string.Empty;

        // Mã phòng, ví dụ: A101
        public string? Code { get; private set; }

        // Sức chứa tối đa
        public int Capacity { get; private set; }

        // Phòng còn hoạt động không
        public bool IsActive { get; private set; } = true;

        // Danh sách lớp học dùng phòng này
        public ICollection<Course> Courses { get; private set; } = new List<Course>();

        private Classroom()
        {
        }

        public Classroom(string name, string? code, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng học không được để trống.");

            if (capacity < 1)
                throw new ArgumentException("Sức chứa phòng học phải lớn hơn 0.");

            Name = name.Trim();
            Code = code?.Trim();
            Capacity = capacity;
            IsActive = true;
        }

        public void Update(string name, string? code, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng học không được để trống.");

            if (capacity < 1)
                throw new ArgumentException("Sức chứa phòng học phải lớn hơn 0.");

            Name = name.Trim();
            Code = code?.Trim();
            Capacity = capacity;
            SetUpdatedAt();
        }
    }
}