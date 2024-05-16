namespace QLNV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var startup = new Startup(builder, builder.Environment);

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app, builder.Environment);

            app.Run();
        }
    }
}
