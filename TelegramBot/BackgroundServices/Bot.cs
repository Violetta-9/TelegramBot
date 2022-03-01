using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.BackgroundServices
{
    public class Bot : BackgroundService
    {
        private readonly TelegramBotClient _client;
        private readonly IEnumerable<IBotCommand> _botCommands;
        private readonly IServiceProvider _serviceProvider;
        public Bot(TelegramBotClient client, IServiceProvider serviceProvider)
        {
            _client = client;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.StartReceiving();//добавляем начало сообщений 

            _client.OnMessage += OnMessageHandler;
         
            return Task.CompletedTask;
        }

        private async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var commands = scope.ServiceProvider.GetRequiredService<IEnumerable<IBotCommand>>();

            var msg = e.Message;
            if (msg.Text != null)
            {
                var commandAlias = msg.Text.Split(" ")[0].ToUpper();

                var command = commands.FirstOrDefault(x => x.Alias.Contains(commandAlias));

                if (command != null)
                {
                    await command.ExecuteAsync(msg);
                }
            }
        }
    }
}
