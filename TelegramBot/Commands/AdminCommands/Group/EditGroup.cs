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

namespace TelegramBot.Commands.AdminCommands.Group
{
    public class EditGroup : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"EDITGROUP"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;

        public EditGroup(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = {' ', '|'};
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 3)
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "неправильный формат",
                    cancellationToken: cancellationToken);
                return;
            }
            var group = _db.Groups.FirstOrDefault(x => x.Id == Convert.ToInt32(strArray[1]));

            if (group != null)
            {
                group.Title = strArray[2].ToUpper();
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, "Успешно обнавленно", cancellationToken: cancellationToken);

            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Записи под данным ID нет",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
