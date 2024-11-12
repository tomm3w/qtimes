using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(QTimes.WebApp.Areas.Identity.IdentityHostingStartup))]
namespace QTimes.WebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}