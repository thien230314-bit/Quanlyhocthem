using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quanlyhocthem.Application
{
    // Đăng ký các service thuộc tầng Application
    public static class DependencyInjection
    {
        // Hàm Program.cs đang gọi
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }

        // Giữ thêm tên AddApplication để tránh vỡ code nếu chỗ khác đang gọi
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddApplicationServices();
        }
    }
}