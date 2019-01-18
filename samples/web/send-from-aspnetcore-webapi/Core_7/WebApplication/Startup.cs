using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Startup
{
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddLogging(loggingBuilder => loggingBuilder.AddDebug());
    }


    public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
    {
        app.UseMvc(routeBuilder => routeBuilder.MapRoute(name: "default",
            template: "{controller=SendMessage}/{action=Get}"));
    }
}