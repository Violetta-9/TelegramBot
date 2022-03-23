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

namespace TelegramBot.Commands.AdminCommands.TimeTable{
    public class EditCommand:IBotCommand
    {
        public string[] Alias { get; set; } = new[] { "EDIT" };
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public EditCommand(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var text = msg.Text.Trim();
            var textWhithOutCommand = text.Remove(0, 5);
            char[] separators = { '|' };
            var strArray = textWhithOutCommand.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.FirstOrDefault(x => x.Title == strArray[4].ToUpper());
            var currentTimeTable=_db.TimeTables.FirstOrDefault(x => x.Id == Convert.ToInt32(strArray[0]));
            if (currentTimeTable != null)
            {
                EvenWeek even;
                DayOfWeek day = 0;
                if (Convert.ToInt32(strArray[3]) == 1)
                {
                    even = EvenWeek.Even;
                }
                else
                {
                    even = EvenWeek.NotEven;
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
                        await _client.SendTextMessageAsync(msg.Chat.Id, "не правильный формат дня недели", cancellationToken: cancellationToken);
                        break;
                }
                currentTimeTable.LessonsOfTheDay = strArray[1];
                currentTimeTable.Week = day;
                currentTimeTable.EvenWeek = even;
                currentTimeTable.Group = group;
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Успешно обнавленно", cancellationToken: cancellationToken);

            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Записи под данным ID нет", cancellationToken: cancellationToken);
            }
           
        }
    }
}
