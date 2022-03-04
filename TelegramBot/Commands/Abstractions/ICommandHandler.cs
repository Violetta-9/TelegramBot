using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Abstractions
{
    public interface ICommandHandler
    {
         Task HandleAsync(Message msg, CancellationToken cancellationToken= default);
    }
}
