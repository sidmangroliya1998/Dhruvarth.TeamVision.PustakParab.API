using Dhruvarth.TeamVision.PustakParab.API.JWTAuth;
using Dhruvarth.TeamVision.PustakParab.DbService;
using Dhruvarth.TeamVision.PustakParab.Models;
using Dhruvarth.TeamVision.PustakParab.Services;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Services
            var jwtTokenConfig = builder.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            builder.Services.AddSingleton(jwtTokenConfig);
            builder.Services.AddHostedService<JwtRefreshTokenCache>(); //For running on backgrond and remove in active users 
            builder.Services.AddSingleton<ISqlDbConn>(_ => new SqlDbConn(builder.Configuration.GetSection("AppSettings").Get<AppSettings>().ConnectionString));

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.ToString());
        }
    }
}