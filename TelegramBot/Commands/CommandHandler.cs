using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;
using TelegramBot.DataAccess;
using TelegramBot.Domain.Models;

namespace TelegramBot.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly ApplicationDbContext _db;
        public CommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task HandleAsync(Message msg, CancellationToken cancellationToken = default)
        {
            if (!_db.Users.Any(x => x.IdChat == msg.Chat.Id))
            {
                _db.Users.Add(new Domain.Models.User(msg.Chat.Id, msg.Text));
            }
        }
    }
}
