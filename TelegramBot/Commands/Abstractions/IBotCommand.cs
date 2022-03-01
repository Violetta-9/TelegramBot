using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Commands.Abstractions
{
    public interface IBotCommand
    {
        public string[] Alias { get; set; }
        Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default);
    }
}
