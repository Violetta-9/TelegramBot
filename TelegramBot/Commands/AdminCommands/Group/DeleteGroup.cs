using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands.AdminCommands.Group
{
    public class DeleteGroup : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"DELETEGROUP"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;

        public DeleteGroup(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = {' ', '|'};
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            var currentGroup = _db.Groups.FirstOrDefault(x => x.Id == Convert.ToInt32(strArray[1]));
            if (currentGroup != null)
            {
                _db.Groups.Remove(currentGroup);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Успешно удаленно",
                    cancellationToken: cancellationToken);

            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Записи под данным ID нет",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
