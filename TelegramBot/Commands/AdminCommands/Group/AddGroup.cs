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
    public class AddGroup:IBotCommand
    {
        public string[] Alias { get; set; } = new[] { "ADDGROUP" };
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public AddGroup(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = { ' ', '|' };
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.FirstOrDefault(x => x.Title == strArray[1].ToUpper());

            if (group != null)
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Группа  существует", cancellationToken: cancellationToken);
                return;
            }

            var newGroup = new Domain.Models.Group(strArray[1].ToUpper());

            _db.Groups.Add(newGroup);
            await _db.SaveChangesAsync(cancellationToken);
            await _client.SendTextMessageAsync(msg.Chat.Id, "Запись успешно добавленна",
                cancellationToken: cancellationToken);

        }
    }
}
