using BlockChain.Configuration;

using Microsoft.Extensions.Configuration;

using Serilog;

using StructureMap;

namespace BlockChain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var blockChainConfiguration = configuration.GetSection("BlockChain").Get<BlockChainConfiguration>();

            var container = ConfigureIoC();
            container.Configure(o => o.For<BlockChainConfiguration>().Use(blockChainConfiguration));

            var app = container.GetInstance<Application>();
            app.Run();
        }

        private static IContainer ConfigureIoC()
        {
            var container = new Container(
                config =>
                {
                    config.Scan(
                        o =>
                        {
                            o.TheCallingAssembly();
                            o.WithDefaultConventions();
                        });
                    config.AddRegistry(new CompositionRoot());
                });

            return container;
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return configuration;
        }
    }
}
