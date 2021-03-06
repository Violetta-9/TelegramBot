using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands.AdminCommands.TimeTable
{
    public class DeleteCommand:IBotCommand
    {
        public string[] Alias { get; set; } = new[] { "DELETE" };
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public DeleteCommand(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = { ' ', '|' };
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
         
            var currentTimeTable = _db.TimeTables.FirstOrDefault(x => x.Id == Convert.ToInt32(strArray[1]));
            if (currentTimeTable != null)
            {
                _db.TimeTables.Remove(currentTimeTable);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Успешно удаленно", cancellationToken: cancellationToken);

            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Записи под данным ID нет", cancellationToken: cancellationToken);
            }

        }
    }
    }