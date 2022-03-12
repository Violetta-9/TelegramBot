using System;
using System.Collections.Generic;
using System.Linq;

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


            
            services.AddControllers();

            services.AddScoped<IBotCommand, TestBotCommand>();
            services.AddScoped<IBotCommand, MenuBotCommand>();
            services.AddScoped<IBotCommand, SetGroup>();
            services.AddScoped<IBotCommand, SetCity>();
            services.AddScoped<IBotCommand, WeatherCommand>();
            services.AddApplication();
;            //services.AddScoped<IBotCommand, Test2BotCommandNew>();
           
            services.AddHostedService<Bot>();
            
            services.AddSingleton<TelegramBotClient>(new TelegramBotClient(Configuration.GetSection("Telegram:Token").Get<string>()));

            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetSection("ConnectionString:DefaultConnection").Get<string>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
