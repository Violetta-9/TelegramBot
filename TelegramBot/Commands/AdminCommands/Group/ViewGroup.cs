using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands.AdminCommands.Group
{
    public class ViewGroup:IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"VIEWGROUP"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;

        public ViewGroup(TelegramBotClient client, ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            char[] separators = {' ', '|'};
            var strArray = msg.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var group = _db.Groups.ToArray();
            var str = "";
            foreach (var item in group)
            {
                str += $"Id: {item.Id}\nНазвание: {item.Title}\n\n";
            }

            await _client.SendTextMessageAsync(msg.Chat.Id, str, cancellationToken: cancellationToken);

        }
    }
}
