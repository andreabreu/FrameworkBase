using Framework.Core.Repository.Interfaces;
using Framework.Core.Repository.UoW;
using Framework.Web.Test.Configuration;
using Framework.Web.Test.Data;
using Framework.Web.Test.Data.Repository;
using Framework.Web.Test.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Web.Test
{
    public class Startup
    {
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(hostEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            ApiConfig.LogConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Domain"];
                    options.Audience = Configuration["Auth0:Audience"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", Configuration["Auth0:Domain"])));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddApiConfiguration(Configuration);

            services.AddMessageBusConfiguration(Configuration);

            services.AddSwaggerConfiguration(Configuration);


            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITestRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);
        }
    }

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }

    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }


}

