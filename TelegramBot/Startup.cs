using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Application;
using Telegram.Application.Contracts;
using Telegram.Bot;
using Telegram.DataAccess;
using TelegramBot.BackgroundServices;
using TelegramBot.Commands;
using TelegramBot.Commands.Abstractions;
using TelegramBot.HostedServices;


namespace TelegramBot
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
        {  services.AddHostedService<MigrationHostedService>();
services.AddHostedService<Bot>();
                      

            services.AddControllers();
            services.AddApplication();
;         
           
         
            
            services.AddSingleton<TelegramBotClient>(new TelegramBotClient(Configuration.GetSection("Telegram:Token").Get<string>()));

            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetSection("ConnectionString:DefaultConnection").Get<string>()));
          
            services.AddHangfire(config => config.UseMemoryStorage());
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(StartCommand))
                    .AddClasses(classes => classes.AssignableTo(typeof(IBotCommand)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            // Add the processing server as IHostedService
            services.AddHangfireServer();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard();
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();
         
            ConfigureJobs();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
        private void ConfigureJobs()
        {
            RecurringJob.AddOrUpdate<ScheduledTask.ScheduledTask>("SendTimeTable", x => x.SendTimeTable(),
                "14 19 * * *", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<ScheduledTask.ScheduledTask>("SendWeather", x => x.SendWeather(),
                "13 19 * * *", TimeZoneInfo.Local);
        }
    }
}
