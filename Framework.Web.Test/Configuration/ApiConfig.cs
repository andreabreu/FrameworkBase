using Framework.Core.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Framework.Web.Test.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("any",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("any");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void LogConfiguration(this IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MongoDB(config.GetConnectionString("Log"), collectionName: "WebApiLogs")
                .CreateLogger();
        }
    }
}
