using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SomeBasicSettings.Middleware;
using System.Threading.Tasks;

namespace SomeBasicSettings
{
public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(configure=>
            {
                configure.UseMiddleware<BearerAuthenticationMiddleware>();
                configure.UseMiddleware<AuthorizationMiddleware>();
            })
            .Build();

        host.Run();
    }
}
}