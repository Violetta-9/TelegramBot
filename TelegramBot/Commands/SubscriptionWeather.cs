using System;
using System.Collections.Generic;
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
    public class SubscriptionWeather : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/SUBSCRIPTIONWEATHER"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        public SubscriptionWeather(TelegramBotClient client,ApplicationDbContext db)
        {
            _client = client;
            _db = db;
        }
        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var check = _db.Subscriptions.Include(x => x.User).Where(x => x.User.IdChat == msg.Chat.Id)
                .FirstOrDefault(x => x.SubscriptionItem == SubscriptionItem.Weather);
            if (check != null)
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Вы уже подписаны :)", cancellationToken: cancellationToken);
                return;
            }
            var person=_db.Users.Where(x => x.IdChat == msg.Chat.Id).FirstOrDefault(x => x.City != null);
            if (person != null)
            {
                var subscription = new Subscription(person.Id,SubscriptionItem.Weather);
               await _db.Subscriptions.AddAsync(subscription,cancellationToken);
               await _db.SaveChangesAsync(cancellationToken);
               await _client.SendTextMessageAsync(msg.Chat.Id, $"Вы успешно подписались на рассылку погоды\n В 7.00 мы пришлем Вам погоду в городе {person.City.Substring(0, 1).ToUpper() + person.City.Substring(1, person.City.Length - 1).ToLower()}", cancellationToken: cancellationToken);
            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Упс!\nУстановите город: setcity + ваш город", cancellationToken: cancellationToken);
            }

        }
    }
}
