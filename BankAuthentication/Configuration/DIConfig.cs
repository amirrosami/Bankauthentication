using BankAuthentication.ExtraServices;
using BankAuthentication.Repository;
using BankAuthentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BankAuthentication.Configuration
{
    public static class DIConfig
    {
        public static IServiceCollection AddBankService(this IServiceCollection services)
        {
             services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<BankContext>(op =>
            op.UseSqlServer("Data Source=gina.iran.liara.ir,30640;Initial Catalog=BankDB;User Id=sa;Password=ivT6QmlFY6Y5q7hfdnaTgoYQ;"));
            services.AddScoped<AuthenticationServices>();
            services.AddScoped<FileManagementService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<IRabbitMqPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
