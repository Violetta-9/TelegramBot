using System;
using Hangfire;
using Hangfire.PostgreSql;
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
using TelegramBot.Commands.AdminCommands;
using TelegramBot.Commands.AdminCommands.Group;
using TelegramBot.Commands.AdminCommands.TimeTable;
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
        {
            services.AddHostedService<Bot>();
            services.AddHostedService<MigrationHostedService>();

            services.AddControllers();

            services.AddScoped<IBotCommand, TestBotCommand>();
            services.AddScoped<IBotCommand, MenuBotCommand>();
            services.AddScoped<IBotCommand, SetGroup>();
            services.AddScoped<IBotCommand, SetCity>();
            services.AddScoped<IBotCommand, WeatherCommand>();
            services.AddScoped<IBotCommand, SubscriptionWeather>();
            services.AddScoped<IBotCommand, StartCommand>();
            services.AddScoped<IBotCommand, SubscriptionTimeTable>();
            services.AddScoped<IBotCommand, AdminCommand>();
            services.AddScoped<IBotCommand, AddTimeTable>();
            services.AddScoped<IBotCommand, ViewCommand>();
            services.AddScoped<IBotCommand, DeleteCommand>();
            services.AddScoped<IBotCommand, EditCommand>();

            services.AddScoped<IBotCommand, AddGroup>();
            services.AddScoped<IBotCommand, ViewGroup>();
            services.AddScoped<IBotCommand, DeleteGroup>();
            services.AddScoped<IBotCommand, EditGroup>();
            


            services.AddApplication();
;            //services.AddScoped<IBotCommand, Test2BotCommandNew>();
           
         
            
            services.AddSingleton<TelegramBotClient>(new TelegramBotClient(Configuration.GetSection("Telegram:Token").Get<string>()));

            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetSection("ConnectionString:DefaultConnection").Get<string>()));
            // services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetSection("ConnectionString:DefaultConnection").Get<string>()));
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(Configuration.GetSection("ConnectionString:DefaultConnection").Get<string>(), new PostgreSqlStorageOptions()
                {
                    
                    InvisibilityTimeout = TimeSpan.FromMinutes(5),
                    DistributedLockTimeout = TimeSpan.FromMinutes(5),
                    
                    UseNativeDatabaseTransactions = true,
                    PrepareSchemaIfNecessary = true

        }));

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
