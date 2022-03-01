using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands
{
    public class WeatherCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/WEATHER"};


        public Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
