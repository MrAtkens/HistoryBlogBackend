using GeekBlog.Contracts.Options;
using GeekBlog.DataAccess.Domain;
using GeekBlog.DataAccess.Providers;
using GeekBlog.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GeekBlog.Api.Admin
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
                .AddJsonFile("appsettings.CoreConfigurations.json")
                .Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext
            services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(
                    _configuration.GetConnectionString("DevConnection")));

            // Providers
            services.AddTransient<EntityAdminProvider>();

            // Authentication
            services.Configure<SecretOption>(_configuration.GetSection("Secrets"));

            services.AddTransient<AdminAuthenticationService>();

            
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "f17bfca3-9faa-495d-97cc-99fba7bc9862";
            });
            
            services.AddMemoryCache();

            // configure jwt authentication
            var secrets = _configuration.GetSection("Secrets");

            var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret"));
            services.AddAuthentication(x =>
                {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });


            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GeekBlog.Admin",
                    Description = "WebApi",

                });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "����� JWT ������� ����������� ��������� 'Bearer ' � ��������!",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }

                    }
                });
            });

            services.AddCors(o => 
            {
                o.AddPolicy(CorsOrigins.FrontPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://localhost:4343")
                        .AllowAnyHeader()
                        .WithMethods("GET")
                        .WithMethods("POST")
                        .AllowCredentials();
                });

                o.AddPolicy(CorsOrigins.AdminPanelPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:4343")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog.Api.Admin v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
