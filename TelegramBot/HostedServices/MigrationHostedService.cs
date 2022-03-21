using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.DataAccess;

namespace TelegramBot.HostedServices
{
    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider _services;

        public MigrationHostedService(IServiceProvider services)
        {
            _services = services;
        }
        public  async Task StartAsync(CancellationToken cancellationToken)
        {
            await Migrate<ApplicationDbContext>(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task Migrate<T>(CancellationToken cancellationToken) where T:DbContext
        {
             using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<T>();
            await db.Database.MigrateAsync(cancellationToken);


        }
    }
}
