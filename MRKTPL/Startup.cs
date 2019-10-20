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
using MRKTPL.Services.RoleServices;
using MRKTPL.Services.SendMailServices;
using MRKTPL.Services.UserServices;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        // This method gets called by the runtime. Use this method to add services to the container.
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
            //services.AddAutoMapper();
            //Mapper.Initialize(cfg => cfg.AddProfile<MappingsProfile>());


            #endregion
            // configure jwt authentication
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
            //It creates the instance for 1st time & 
            //reuses the same object in all calls.

            services.AddTransient<IUserServices, UserServices>();//It creates instance each time they are requested.
            services.AddTransient<IRoleServices, RoleServices>();
            services.AddTransient<IUsersInRoles, RoleRightServices>();

            services.AddTransient<ILogger, Logger>();
            services.AddTransient<ISendMailServices, SendMailServices>();

            //services.AddScoped<IErrorLoggerManager, ErrorLoggerManager>();
            services.AddScoped<ErrorLoggerManager>();


            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                ActionContext actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            //It ceates 1 instance per each http request but use the same instance in other calls within the scope.
            //It is equivalent to AddSingleton.
            #endregion

            //#region Cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("http://localhost:4200")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials()
            //            .WithExposedHeaders("X-Pagination"));
            //});
            //#endregion
            #region ExceptionFilter


            services.AddMvc(options => { options.Filters.Add(typeof(CustomExceptionFilterAttribute)); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #endregion
            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ErrorLoggerManager errorLogger,IOptions<SendMailSettings>emailSetting)//, IOptions<SendMail> sendMail
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.CustomExceptionMiddleware(errorLogger, emailSetting);
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            //app.UseCors("CorsPolicy");

            string[] origins = new string[] { "http://localhost:4200" };
            app.UseCors(b => b.AllowAnyMethod().AllowAnyHeader().WithOrigins(origins));
            app.UseMvc();
        }

    }
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Main API v1.0", Version = "v1.0" });

                //Locate the XML file being generated by ASP.NET...
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    //... and tell Swagger to use those XML comments.
                    c.IncludeXmlComments(xmlPath);
                }


                // Swagger 2.+ support
                Dictionary<string, IEnumerable<string>> security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                //Must require for swagger version > 2.0
                c.AddSecurityRequirement(security);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Gym Web API v1.0");
                c.DocumentTitle = "Title Documentation";
                //Reference link : https://stackoverflow.com/questions/22008452/collapse-expand-swagger-response-model-class
                //Reference link : https://swagger.io/docs/open-source-tools/swagger-ui/usage/deep-linking/
                //  c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                // c.DocExpansion(DocExpansion.Full);
                //    //Reference document: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio
                //    //To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
