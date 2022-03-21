using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;
using TelegramBot.Domain.Models;

namespace TelegramBot.Commands.AdminCommands.TimeTable
{
    public class AddTimeTable : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"ADD"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public AddTimeTable(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = {' ', '|'};
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.FirstOrDefault(x => x.Title == strArray[4].ToUpper());
            if (group != null)
            {
                EvenWeek even;
                DayOfWeek day = 0;
                if (Convert.ToInt32(strArray[3]) == 1)
                {
                    even = EvenWeek.Even;
                }
                else
                {
                    even = EvenWeek.Even;
                }

                switch (strArray[2])
                {
                    case "1":
                        day = DayOfWeek.Monday;
                        break;
                    case "2":
                        day = DayOfWeek.Tuesday;
                        break;
                    case "3":
                        day = DayOfWeek.Wednesday;
                        break;
                    case "4":
                        day = DayOfWeek.Thursday;
                        break;
                    case "5":
                        day = DayOfWeek.Friday;
                        break;
                    default:
                        await _client.SendTextMessageAsync(msg.Chat.Id, "не правильный формат дня недели",
                            cancellationToken: cancellationToken);
                        break;
                }

                var newTimeTable = new Domain.Models.TimeTable(strArray[1], day, even, group);
                _db.TimeTables.Add(newTimeTable);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Запись успешно добавленна",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
