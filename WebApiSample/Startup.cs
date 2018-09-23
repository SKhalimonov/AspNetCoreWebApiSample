using AutoMapper;
using MessageSender.Configurations;
using MessageSender.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System;
using System.Security.Claims;
using System.Text;
using WebApiSample.Core.Configuration;
using WebApiSample.Core.Identity;
using WebApiSample.Core.Services;
using WebApiSample.Data;
using WebApiSample.Data.Context;
using WebApiSample.Data.Core;
using WebApiSample.Data.Core.Repositories;
using WebApiSample.Data.Repositories;
using WebApiSample.Services;
using WebApiSample.Services.Identity;

namespace WebApiSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAutoMapper(typeof(Startup));

            Config config = GetConfig();
            IdentityConfig identityConfig = config.Identity;
            ConfigureLogging(config);
            ConfigureAuthenticationOptions(services, identityConfig);

            services.AddSingleton(typeof(Config), config);
            services.AddSingleton(typeof(BaseEmailConfig), config.Email);
            services.AddSingleton(typeof(IdentityConfig), config.Identity);

            services.AddDbContextPool<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(config.ConnectionStrings.WebApiSampleDatabase)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)));

            services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddAuthorization(AddAuthorizationOptions);

            ConfigureOwnDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            app.UseAuthentication();
            app.UseMvc();

            DbInitializer.Initialize(context);
            app.UseCors("AllowAnyOrigin");
        }

        private void AddAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy("ForAdmin", policy =>
                          policy.RequireClaim(ClaimTypes.Role, "True"));
        }

        private void ConfigureAuthenticationOptions(IServiceCollection services, IdentityConfig identityConfig)
        {
            var key = Encoding.ASCII.GetBytes(identityConfig.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = identityConfig.RequireHttpsMetadata;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        private void ConfigureOwnDependencies(IServiceCollection services)
        {
            services.AddScoped<IEndUser, EndUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private Config GetConfig()
        {
            Config config = new Config();
            Configuration.Bind(config);

            return config;
        }

        private void ConfigureLogging(Config config)
        {
            LogEventLevel minimumLogLevel = LogEventLevel.Information;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLogLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(a =>
                {
                    a.RollingFile(
                        "Logs\\memorycare-{Date}.txt",
                        minimumLogLevel,
                        "[{Timestamp:HH:mm:ss.fff} {Level:u3} {RequestId}] '{RequestPath}' {Message:lj} {NewLine}{Properties:j} {NewLine}{Exception}");
                })
                .CreateLogger();
        }
    }
}
