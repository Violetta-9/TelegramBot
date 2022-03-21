using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Application.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Commands.Abstractions;

namespace TelegramBot.Commands
{
    public class AdminCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/ADMIN"};
        public readonly TelegramBotClient _client;
        public readonly string _str;

        public AdminCommand(TelegramBotClient client,IOptions<Settings> str)
        {
            _client = client;
            _str = str.Value.AdminId;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            if (msg.Chat.Id == Convert.ToInt32(_str))
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Команды:\n   add  расписание со временим | день недели | четность  | группа \n_________\n День недели:\n1-пн\n2-вт\n3-ср\n4-чт\n5-пт\n _________\n Четность:\n1-четная \n2-нечетная\n\n   view  группа (view | группа | день недели)\n\n   delete  Id записи\n\n   edit  Id записи | новое расписание | день недели | четность | группа", cancellationToken: cancellationToken);
                return;

            }
            await _client.SendTextMessageAsync(msg.Chat.Id, "Хе-хе,Вы не админ)", cancellationToken: cancellationToken);

        }
    }
}
