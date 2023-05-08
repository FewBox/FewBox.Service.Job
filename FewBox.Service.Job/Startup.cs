using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FewBox.Core.Utility.Net;

namespace FewBox.Service.Job
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            NetworkUtility.IsCertificateNeedValidate = false;
            NetworkUtility.IsEnsureSuccessStatusCode = false;
            var endpointEvents = Configuration.GetSection("EndpointEvents").Get<IList<EndpointEvent>>();
            services.AddSingleton(endpointEvents);
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddConfiguration(this.Configuration.GetSection("Logging"));
            })
            .Configure<LoggerFilterOptions>(this.Configuration);
            services.AddSingleton<IJob, Job>();
        }
    }
}