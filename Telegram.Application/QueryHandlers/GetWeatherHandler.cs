using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Telegram.Application.Contracts;
using Telegram.Application.Queries;

namespace Telegram.Application.QueryHandlers
{
    public class GetWeatherHandler : IRequestHandler<GetWeather, WeatherInfo>
    {
        public readonly string _token;

        public GetWeatherHandler(IOptions<Settings> token)
        {
            _token = token.Value.WeatherToken;
        }
        public async Task<WeatherInfo> Handle(GetWeather request, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var requestForWeather = new HttpRequestMessage()
            {
                RequestUri =
                    new Uri(
                        $"https://api.openweathermap.org/data/2.5/onecall?lat={request.Latitude}&lon={request.Longitude}&units=metric&exclude=current,minutely,hourly,alerts&appid={_token}&lang=ru"),
                Method = HttpMethod.Get
            };
            using (var response = await client.SendAsync(requestForWeather))
            {
                response.EnsureSuccessStatusCode();
                var weatherInfoDaily = await response.Content.ReadAsStringAsync();
                var weatherInfodailyToClass = JsonConvert.DeserializeObject<WeatherInfo>(weatherInfoDaily);
                return weatherInfodailyToClass;
            }

        }
    }
}
