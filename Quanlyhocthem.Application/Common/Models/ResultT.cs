namespace Quanlyhocthem.Application.Common.Models
{
    // Result<T> dùng cho các thao tác cần trả dữ liệu về
    public class Result<T>
    {
        // Cho biết thao tác có thành công không
        public bool Succeeded { get; private set; }

        // Dữ liệu trả về nếu thành công
        public T? Data { get; private set; }

        // Thông báo lỗi nếu thất bại
        public string? Error { get; private set; }

        // Constructor private để bắt buộc dùng Success hoặc Failure
        private Result(bool succeeded, T? data, string? error)
        {
            Succeeded = succeeded;
            Data = data;
            Error = error;
        }

        // Trả kết quả thành công có dữ liệu
        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null);
        }

        // Trả kết quả thất bại
        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, default, error);
        }
    }
}