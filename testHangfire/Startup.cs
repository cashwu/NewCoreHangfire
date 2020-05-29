using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.Dark;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace testHangfire
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
            services.AddControllers();

            services.AddHangfire(config =>
            {
                config
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSQLiteStorage()
                    .WithJobExpirationTimeout(TimeSpan.FromDays(2));

                config.UseConsole();

                config.UseFilter(new LogEverythingAttribute());

                // config.UseDarkDashboard();

                // config.UseFilter(new ProlongExpirationTimeAttribute());
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ConstHelper>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(app.ApplicationServices.GetAutofacRoot());

            app.UseHangfireDashboard($"/dashboard", new DashboardOptions
            {
                StatsPollingInterval = 2000,
                DisplayStorageConnectionString = true,

                // Authorization = new[] { new CustomAuthorizationFilter() },
                IsReadOnlyFunc = context => true
            });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = "job test",
                WorkerCount = 2,
                Queues = new[] { "low" },
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class ConstHelper
    {
        public int Test()
        {
            return 123;
        }
    }
}