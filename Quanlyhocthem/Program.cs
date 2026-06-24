using Microsoft.EntityFrameworkCore;
using Quanlyhocthem.Domain.Entities;
using Quanlyhocthem.Domain.Enums;
using Quanlyhocthem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quanlyhocthem.Application;
using Quanlyhocthem.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký Controller để API nhận request từ Postman
builder.Services.AddControllers();

// Đăng ký tầng Application: MediatR Command/Query Handler
builder.Services.AddApplicationServices();

// Đăng ký tầng Infrastructure: DbContext, SQL Server, JWT service
builder.Services.AddInfrastructureServices(builder.Configuration);

// Lấy cấu hình JWT từ appsettings.Development.json khi chạy Development
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtSecretKey))
{
    throw new InvalidOperationException("Chưa cấu hình Jwt:SecretKey trong appsettings.Development.json.");
}

// Cấu hình xác thực bằng JWT Bearer Token
builder.Services.AddAuthentication(options =>
{
    // Dùng JWT Bearer làm cơ chế xác thực mặc định
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Quy tắc kiểm tra token gửi từ Postman
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Kiểm tra khóa ký token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),

        // Kiểm tra Issuer
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        // Kiểm tra Audience
        ValidateAudience = true,
        ValidAudience = jwtAudience,

        // Kiểm tra token hết hạn
        ValidateLifetime = true,

        // Không cộng thêm thời gian lệch
        ClockSkew = TimeSpan.Zero
    };
});

// Đăng ký phân quyền theo role: Admin, Teacher, Student
builder.Services.AddAuthorization();

var app = builder.Build();
// Tạo tài khoản Admin mặc định để test Postman lần đầu
// UserName: admin
// Password: 2412
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Tự động apply migration nếu database chưa cập nhật
    await dbContext.Database.MigrateAsync();

    // Nếu chưa có Admin thì tạo Admin mặc định
    var adminExists = await dbContext.Users
        .AnyAsync(x => x.Role == Role.Admin);

    if (!adminExists)
    {
        var admin = new User(
            "admin",
            "2412",
            "Quản trị viên",
            "admin@quanlyhocthem.local",
            null,
            Role.Admin
        );

        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }
}

// Chuyển HTTP sang HTTPS
app.UseHttpsRedirection();

// Xác thực user từ JWT token trước
app.UseAuthentication();

// Sau đó kiểm tra quyền role
app.UseAuthorization();

// Map toàn bộ API Controller
app.MapControllers();

app.Run();