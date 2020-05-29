using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                // config.UseFilter(new ProlongExpirationTimeAttribute());
            });

            // services.AddHangfireServer();

            // GlobalConfiguration.Configuration
            //                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //                    .UseSimpleAssemblyNameTypeSerializer()
            //                    .UseRecommendedSerializerSettings()
            //                    .UseSQLiteStorage("", new SQLiteStorageOptions());
            // .UseSqlServerStorage("Database=Hangfire.Sample; Integrated Security=True;", new SqlServerStorageOptions
            // {
            //     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //     QueuePollInterval = TimeSpan.Zero,
            //     UseRecommendedIsolationLevel = true,
            //     UsePageLocksOnDequeue = true,
            //     DisableGlobalLocks = true
            // })
            // .UseBatches()
            // .UsePerformanceCounters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
}