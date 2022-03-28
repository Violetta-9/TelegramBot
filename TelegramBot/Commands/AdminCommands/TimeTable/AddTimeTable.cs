using System;
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
            var text = msg.Text.Trim();
            var textWhithOutCommand = text.Remove(0, 3);
            char[] separators = {'|'};
            var strArray = textWhithOutCommand.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.FirstOrDefault(x => x.Title == strArray[3].ToUpper());
            if (group != null)
            {
                EvenWeek even;
                DayOfWeek day = 0;
                if (Convert.ToInt32(strArray[2]) == 1)
                {
                    even = EvenWeek.Even;
                }
                else
                {
                    even = EvenWeek.NotEven;
                }

                switch (strArray[1])
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

                var newTimeTable = new Domain.Models.TimeTable(strArray[0], day, even, group);
                _db.TimeTables.Add(newTimeTable);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Запись успешно добавленна",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Такой группы нет",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
