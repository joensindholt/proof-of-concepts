using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WCFHelloWorldService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseServiceModel(builder =>
            {
                builder
                    .AddService<HelloWorldService>()
                    .AddServiceEndpoint<HelloWorldService, IHelloWorldService>(new BasicHttpBinding(), "/basichttp")
                    .AddServiceEndpoint<HelloWorldService, IHelloWorldService>(new NetTcpBinding(), "/nettcp");
            });
        }
    }
}