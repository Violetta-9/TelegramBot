using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Telegram.Bot;
using TelegramBot.Application;
using TelegramBot.BackgroundServices;
using TelegramBot.Commands;
using TelegramBot.Commands.Abstractions;
using TelegramBot.DataAccess;

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
;            //services.AddScoped<IBotCommand, Test2BotCommandNew>();
           
            services.AddHostedService<Bot>();
            
            services.AddSingleton<TelegramBotClient>(new TelegramBotClient(Configuration.GetSection("Telegram:Token").Get<string>()));
           
            

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
