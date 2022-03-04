using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;
using TelegramBot.DataAccess;
using TelegramBot.Domain.Models;

namespace TelegramBot.Commands
{
    public class MenuBotCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/TIMETABLE"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public MenuBotCommand(TelegramBotClient client,ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            EvenWeek even;
            DateTime myDateTime = DateTime.Now;
            int firstDayOfYear = (int)new DateTime(myDateTime.Year, 1, 1).DayOfWeek;
            int weekNumber = (myDateTime.DayOfYear + firstDayOfYear) / 7 + 1;
            if (weekNumber % 2 == 0)
            {
                 even = EvenWeek.Even;
            }
            else
            {
                even = EvenWeek.Even;
            }
            var dayOfWeekNow = myDateTime.DayOfWeek;
            
            var timeTableTry = _db.TimeTables.Where(x=>x.Week==dayOfWeekNow).Where(x=>x.EvenWeek==even).ToArray();
            await _client.SendTextMessageAsync(msg.Chat.Id, timeTableTry[0].LessonsOfTheDay);
        }
    }
}
