using GatherTransit.Server;
using GatherTransit.Server.Model;
using Microsoft.Extensions.Configuration;
using System;

namespace GatherTransit
{
    class Program
    {
        static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true);
            Configuration = builder.Build();
            var connConfig = new ConnConfig();
            Configuration.GetSection("ConnConfig").Bind(connConfig);
            Pack pack = new Pack(connConfig.NumConnections, connConfig.ReceiveBufferSize,
                connConfig.Overtime, connConfig.Port, 0xff);
            Console.WriteLine("服务端已准备好!");
            Console.Read();
        }
    }
}
