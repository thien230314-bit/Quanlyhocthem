using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quanlyhocthem.Application.Common.Interfaces;
using Quanlyhocthem.Infrastructure.Persistence;
using Quanlyhocthem.Infrastructure.Services;

namespace Quanlyhocthem.Infrastructure
{
    // Class đăng ký các service của tầng Infrastructure
    public static class DependencyInjection
    {
        // Hàm này sẽ được gọi trong Program.cs của Api
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Lấy chuỗi kết nối SQL Server từ appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Đăng ký AppDbContext dùng SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Khi Application cần IAppDbContext thì dùng AppDbContext
            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

            // Đăng ký service tạo JWT token
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}