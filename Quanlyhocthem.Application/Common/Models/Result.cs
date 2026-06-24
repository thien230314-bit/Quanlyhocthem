namespace Quanlyhocthem.Application.Common.Models
{
    // Result dùng cho các thao tác chỉ cần báo thành công hoặc thất bại
    public class Result
    {
        // Cho biết thao tác có thành công không
        public bool Succeeded { get; private set; }

        // Thông báo lỗi nếu thao tác thất bại
        public string? Error { get; private set; }

        // Constructor private để bắt buộc dùng Success hoặc Failure
        private Result(bool succeeded, string? error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        // Trả về kết quả thành công
        public static Result Success()
        {
            return new Result(true, null);
        }

        // Trả về kết quả thất bại
        public static Result Failure(string error)
        {
            return new Result(false, error);
        }
    }
}