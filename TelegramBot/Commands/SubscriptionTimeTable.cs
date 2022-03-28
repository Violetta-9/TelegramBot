using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;
using TelegramBot.Domain.Models;

namespace TelegramBot.Commands
{
    public class SubscriptionTimeTable : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/SUBSCRIPTIONTIMETABLE"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public SubscriptionTimeTable(TelegramBotClient client,ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var check = _db.Subscriptions.Include(x => x.User).Where(x => x.User.IdChat == msg.Chat.Id)
                .FirstOrDefault(x => x.SubscriptionItem == SubscriptionItem.TimeTable);
            if (check != null)
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Вы уже подписаны :)", cancellationToken: cancellationToken);
                return;
            }
            var person = _db.Users.Include(x=>x.Group).Where(x => x.IdChat == msg.Chat.Id).FirstOrDefault(x => x.Group.Title != null);
            if (person != null)
            {
                var subscription = new Subscription(person.Id, SubscriptionItem.TimeTable);
                await _db.Subscriptions.AddAsync(subscription, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                await _client.SendTextMessageAsync(msg.Chat.Id, $"Вы успешно подписались на рассылку расписания\n В 7.15 мы пришлем Вам расписание {person.Group.Title} группы ", cancellationToken: cancellationToken);
            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Упс!\nУстановите группу: setgroup + вашa группа", cancellationToken: cancellationToken);
            }
        }
    }
}
