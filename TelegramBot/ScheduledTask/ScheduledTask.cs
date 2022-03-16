using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Application.Queries;
using Telegram.Bot;
using Telegram.DataAccess;
using TelegramBot.Domain.Models;

namespace TelegramBot.ScheduledTask
{
    public class ScheduledTask
    {
        private readonly ApplicationDbContext _db;
        private readonly TelegramBotClient _client;
        private readonly IMediator _mediator;

        public ScheduledTask(ApplicationDbContext db, TelegramBotClient client, IMediator mediator)
        {
            _db = db;
            _client = client;
            _mediator = mediator;
        }

        public async Task SendTimeTable()
        {

            var subscriptions = _db.Subscriptions.Include(x => x.User).ThenInclude(x => x.Group).Where(x => x.SubscriptionItem == SubscriptionItem.TimeTable).ToArray();
            var groupGroup = subscriptions.GroupBy(x => x.User.Group.Id).ToArray();


            EvenWeek even;
            DateTime myDateTime = DateTime.Now;
            int firstDayOfYear = (int)new DateTime(myDateTime.Year, 1, 1).DayOfWeek;
            int weekNumber = (myDateTime.DayOfYear + firstDayOfYear) / 7 + 1;
            if (weekNumber % 2 == 0)
            {
                even = EvenWeek.Even;
            }
            else
            {
                even = EvenWeek.Even;
            }
            var dayOfWeekNow = myDateTime.DayOfWeek;


            foreach (var item in groupGroup)
            {
                var timeTableTry = _db.TimeTables.Where(x => x.Group.Id == item.Key).Where(x => x.Week == dayOfWeekNow).Where(x => x.EvenWeek == even).ToArray();

                var id = item.Select(x => x.User.IdChat);

                foreach (var userId in id)
                {
                    await _client.SendTextMessageAsync(userId, timeTableTry[0].LessonsOfTheDay);

                }

            }

        }
        public async Task SendWeather()
        {

            var weatherSubscript = _db.Subscriptions.Include(x => x.User)
                .Where(x => x.SubscriptionItem == SubscriptionItem.Weather).ToArray();
            var weatherInfo = weatherSubscript.GroupBy(x => x.User.City).ToArray();
            foreach (var item in weatherInfo)
            {
                var cordinats = await _mediator.Send(new GetCity(item.Key));
                var weather = await _mediator.Send(new GetWeather(cordinats.Results[0].Locations[1].LatLng.Lat,
                     cordinats.Results[0].Locations[1].LatLng.Lng));
                var idChat = item.Select(x => x.User.IdChat);
                foreach (var idChatItem in idChat)
                {
                    await _client.SendTextMessageAsync(idChatItem,
                        $"Погода в городе {item.Key.Substring(0, 1).ToUpper() + item.Key.Substring(1, item.Key.Length - 1).ToLower()}\n ( {DateTime.Now.ToString("dddd, d MMMM ", CultureInfo.GetCultureInfo("ru-ru"))}):\n" +
                        $"\n" +
                        $"Описание погоды: {weather.Daily[0].Weather[0].Description}\n" +
                        $"Температура от {Math.Round(weather.Daily[0].Temp.MinTemp)} °C до {Math.Round(weather.Daily[0].Temp.MaxTemp)} °C\n" +
                        $"Утром {Math.Round(weather.Daily[0].Temp.Morning)} °C\n" +
                        $"Днем {Math.Round(weather.Daily[0].Temp.Day)} °C\n" +
                        $"Вечером {Math.Round(weather.Daily[0].Temp.Evening)} °C\n" +
                        $"Ночью {Math.Round(weather.Daily[0].Temp.Night)} °C\n" +
                        $"Влажность: {weather.Daily[0].Humidity} %\n" +
                        $"Облачность: {weather.Daily[0].Clouds} %\n" +
                        $"Давление: {weather.Daily[0].Pressure} мм.рт.ст.\n" +
                        $"Ветер: {weather.Daily[0].WindSpeed} м/с");

                }


            }

        }


    }
}
