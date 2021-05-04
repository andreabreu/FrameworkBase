using Framework.Web.Test.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Web.Test.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<MongoContext>();
        }
    }
}
