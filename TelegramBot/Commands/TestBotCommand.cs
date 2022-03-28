using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands
{
    public class TestBotCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"TEST"};
        private readonly TelegramBotClient _client;

        public TestBotCommand(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            await _client.SendTextMessageAsync(msg.Chat.Id, $"Вы отправили {msg.Text}", replyToMessageId: msg.MessageId, cancellationToken: cancellationToken);
        }
    }
}
