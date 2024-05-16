using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLNV.CoreHelper;
using QLNV.Models;
using QLNV.Repositories;
using QLNV.Services;
using System.Text;

namespace QLNV
{
    public class Startup
    {
        public IConfiguration _configuration;

        public Startup(WebApplicationBuilder builder, IWebHostEnvironment env)
        {
            _configuration = builder.Configuration;
        }


        ///=======================================================
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews(); // Đăng ký dịch vụ MVC
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(option =>
            {
                {
                    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "QLNV", Version = "v1" });
                    option.MapType<DateOnly>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Format = "date"
                    });
                }
            }
            );

            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = false;
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddAutoMapper(typeof(Startup));

            AddDI(services);

            var connectionString = _configuration["ConnectionStrings:DefaultConnectionStringDB"];

            services.AddDbContext<QuanLiNhanVienContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("connectionString"))
            );

            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            services.AddAuthentication("Bearer").AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        }
        public void AddDI(IServiceCollection services)
        {
            // Đăng ký các dịch vụ khác cho controller sử dụng
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        ///=======================================================
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Sử dụng trang lỗi cho môi trường Development
            }

            var isUseSwagger = _configuration.GetValue<bool>("UseSwagger", false);
            // Configure the HTTP request pipeline.
            if (isUseSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DefaultModelExpandDepth(-1);
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoCo v1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.UseHttpsRedirection();

            app.UseMiddleware<JwtRefreshMiddleware>();
            app.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}
