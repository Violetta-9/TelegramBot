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
    public class MenuBotCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/TIMETABLE"};
        private readonly TelegramBotClient _client;

        public MenuBotCommand(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            DateTime myDateTime = DateTime.Now;
            int firstDayOfYear = (int)new DateTime(myDateTime.Year, 1, 1).DayOfWeek;
            int weekNumber = (myDateTime.DayOfYear + firstDayOfYear) / 7 + 1;
            await _client.SendTextMessageAsync(msg.Chat.Id, "zzzz");
        }
    }
}
