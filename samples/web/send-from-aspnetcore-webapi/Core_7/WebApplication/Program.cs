using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

public class Program
{
    public static async Task Main()
    {
        #region EndpointConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.SendOnly();

        #endregion

        #region Routing

        var routing = transport.Routing();
        routing.RouteToEndpoint(
            assembly: typeof(MyMessage).Assembly,
            destination: "Samples.ASPNETCore.Endpoint");

        #endregion

        #region EndpointStart

        var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        #endregion

        var hostBuilder = new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();

        #region ServiceRegistration

        hostBuilder.ConfigureServices(services => services.Add(new ServiceDescriptor(typeof(IMessageSession), endpoint)));

        #endregion

        var host = hostBuilder.Build();

        await host.RunAsync();

        await endpoint.Stop();
    }
}