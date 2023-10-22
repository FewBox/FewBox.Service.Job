using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FewBox.Service.Job
{
    public class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                Startup startup = new Startup();
                startup.ConfigureServices(services);
            })
            .Build()
            .Services
            .GetService<IJob>()
            .Execute();
        }
    }
}
