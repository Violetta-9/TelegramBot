using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Application.Queries;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.DataAccess;
using TelegramBot.Commands.Abstractions;


namespace TelegramBot.Commands
{
    public class WeatherCommand : IBotCommand
    {
        public string[] Alias { get; set; } = new[] {"/WEATHER"};
        private readonly TelegramBotClient _client;
        private readonly ApplicationDbContext _db;
        private readonly IMediator _mediator;
        public WeatherCommand(TelegramBotClient client, ApplicationDbContext db,IMediator mediator)
        {
            _mediator = mediator;
            _client = client;
            _db = db;
        }

        public async Task ExecuteAsync(Message msg, CancellationToken cancellationToken = default)
        {
            var check = _db.Users.Where(x => x.IdChat == msg.Chat.Id).FirstOrDefault(x => x.City != null);
            if (check!=null)
            {
                var cityLocation=await _mediator.Send(new GetCity(check.City), cancellationToken);
                var weather = await _mediator.Send(new GetWeather(cityLocation.Results[0].Locations[1].LatLng.Lat,
                    cityLocation.Results[0].Locations[1].LatLng.Lng), cancellationToken);

                await _client.SendTextMessageAsync(msg.Chat.Id, $"Погода в городе {check.City.Substring(0,1).ToUpper()+check.City.Substring(1, check.City.Length-1).ToLower()}\n ( {DateTime.Now.ToString("dddd, d MMMM ", CultureInfo.GetCultureInfo("ru-ru"))}):\n" +
                                                                $"\n" +
                                                                $"Описание погоды: {weather.Daily[0].Weather[0].Description}\n" +
                                                                $"Температура от {Math.Round(weather.Daily[0].Temp.MinTemp)} °C до {Math.Round(weather.Daily[0].Temp.MaxTemp)} °C\n" +
                                                                $"Утром { Math.Round (weather.Daily[0].Temp.Morning)} °C\n" +
                                                                $"Днем {Math.Round( weather.Daily[0].Temp.Day)} °C\n" +
                                                                $"Вечером {Math.Round(weather.Daily[0].Temp.Evening)} °C\n" +
                                                                $"Ночью { Math.Round (weather.Daily[0].Temp.Night)} °C\n" +
                                                                $"Влажность: { weather.Daily[0].Humidity} %\n" +
                                                                $"Облачность: {weather.Daily[0].Clouds} %\n" +
                                                                $"Давление: {weather.Daily[0].Pressure} мм.рт.ст.\n" +
                                                                $"Ветер: {weather.Daily[0].WindSpeed} м/с", cancellationToken: cancellationToken);


            }
            else
            {
                await _client.SendTextMessageAsync(msg.Chat.Id, "Установите город: setcity + ваш город", cancellationToken: cancellationToken);
            }
        }
    }
}
