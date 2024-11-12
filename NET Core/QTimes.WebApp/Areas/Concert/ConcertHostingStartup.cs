using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(QTimes.WebApp.Areas.Concert.ConcertHostingStartup))]
namespace QTimes.WebApp.Areas.Concert
{
    public class ConcertHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}