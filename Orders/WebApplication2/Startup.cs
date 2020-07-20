using Data;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace WebApplication2
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("MyConnectionString");
            services.AddScoped(p => new CoreContext(new DbContextOptionsBuilder<CoreContext>()
                .UseSqlServer(connectionString).Options));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartTwin - Core HTTP API",
                    Version = "v1",
                    Description = "The SmartTwin Core Service HTTP API"
                });
            });

            services.AddScoped<IReaderFileService, ReaderFileService>();
            services.AddScoped<IFileValidatorService, FileValidatorService>();
            services.AddScoped<IParserFileService, ParserFileService>();
            services.AddScoped<IFileHandlerService, FileHandlerService>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IScannerService, ScannerService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var scanner = serviceProvider.GetService<IScannerService>();

            var scheduler = new StartSchedulerScanner(scanner);
            scheduler.RunScheduller();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Error()
                 .WriteTo.File(Configuration.GetValue<string>("Log:FilePath"), rollingInterval: RollingInterval.Day)
                 .CreateLogger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = string.Empty;
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Core.API V1");
            });

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
