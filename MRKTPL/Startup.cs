using log4net.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MRKTPL.Common;
using MRKTPL.Data.Entities;
using MRKTPL.Data.Helper;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;
using MRKTPL.Repository.Mapping;
using MRKTPL.Services.ErrorLoggerServices;
using MRKTPL.Services.RoleServices;
using MRKTPL.Services.SendMailServices;
using MRKTPL.Services.UserServices;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MRKTPL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            #region MyRegion
            string connection = Configuration.GetConnectionString("DatabaseConnStr");
            services.AddDbContext<MarketPlaceCoreContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));

            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var sendMail = Configuration.GetSection("SendMailSettings");
            var mailSetting = sendMail.Get<SendMailSettings>(); 
            services.Configure<SendMailSettings>(sendMail);
            #region Automapper
            MappingsProfile.Initialize();
            #endregion
            
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
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
                        ValidateAudience = false
                    };
                });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IRoleServices, RoleServices>();
            services.AddTransient<IUsersInRoles, RoleRightServices>();
            services.AddTransient<Services.ErrorLoggerServices.ILogger, Logger>();
            services.AddTransient<ISendMailServices, SendMailServices>();
            services.AddScoped<ErrorLoggerManager>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                ActionContext actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
           // services.AddSwagger();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MRKTPL", Version = "v1" });
            //});
            #endregion

            #region ExceptionFilter
            services.AddMvc(options => { options.Filters.Add(typeof(CustomExceptionFilterAttribute)); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                });
            #endregion
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ErrorLoggerManager errorLogger,IOptions<SendMailSettings>emailSetting)//, IOptions<SendMail> sendMail
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            else
            {
                app.UseHsts();
            }
            app.CustomExceptionMiddleware(errorLogger, emailSetting);

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity V1");
            //    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            //});

           // app.UseCustomSwagger();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            string[] origins = new string[] { "http://localhost:4200" };
            app.UseCors(b => b.AllowAnyMethod().AllowAnyHeader().WithOrigins(origins));
            app.UseMvc();
        }

    }
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MRKTPL",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = new System.Uri("https://www.talkingdotnet.com"),
                    Contact = new OpenApiContact() { Name = "Talking Dotnet", Email = "contact@mrktpl.com" }
                });
            });
        }
        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
               
            });
        }
    }

}
