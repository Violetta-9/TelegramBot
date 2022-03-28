using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands.AdminCommands.TimeTable
{
    public class ViewCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"VIEW"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;

        public ViewCommand(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = { ' ', '|' };
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.FirstOrDefault(x => x.Title == strArray[1].ToUpper());
            Domain.Models.TimeTable[] timetableArray;
            string str = "";
            if (strArray.Length == 2)
            {
               
                timetableArray= _db.TimeTables.Include(x => x.Group).Where(x => x.Group.Id == group.Id).ToArray();
            }
            else
            {
                DayOfWeek day = 0;
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
               timetableArray=  _db.TimeTables.Include(x => x.Group).Where(x => x.Group.Id == group.Id).Where(x=>x.Week==day).ToArray();
               
            }

            if (timetableArray.Length==0)
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Расписание этой группы отсутствует", cancellationToken: cancellationToken);
                return;
            }
            foreach (var item in timetableArray)
            {
                str += $"   Id: {item.Id}\n   Расписание: {item.LessonsOfTheDay}\n   День недели: {item.Week}\n   Четность: {item.EvenWeek}\n   Группа: {item.Group.Title}  \n\n\n";
            }
            await _client.SendTextMessageAsync(msg.Chat.Id, str, cancellationToken: cancellationToken);

        }
    }
}
