using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/START"};
        private readonly TelegramBotClient _client;

        public StartCommand(TelegramBotClient client)
        {
            _client = client;
        }
        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            await _client.SendTextMessageAsync(msg.Chat.Id, $"Добро пожаловать,{msg.Chat.FirstName}", cancellationToken: cancellationToken);
        }
    }
}
