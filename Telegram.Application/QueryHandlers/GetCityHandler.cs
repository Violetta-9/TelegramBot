using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Telegram.Application.Contracts;
using Telegram.Application.Queries;

namespace Telegram.Application.QueryHandlers
{
    public class GetCityHandler : IRequestHandler<GetCity, CityInfo>
    {
        public readonly string _token;

        public GetCityHandler(IOptions<Settings> token)
        {
            _token = token.Value.CityToken;
        }
        public async Task<CityInfo> Handle(GetCity request, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var requstForCity = new HttpRequestMessage()
            {
                RequestUri =
                    new Uri($"http://open.mapquestapi.com/geocoding/v1/address?key={_token}&location={request.Name}"),
                Method = HttpMethod.Get
            };
            using (var response = await client.SendAsync(requstForCity, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var cityInfo = await response.Content.ReadAsStringAsync();
                var cityInfoToClass = JsonConvert.DeserializeObject<CityInfo>(cityInfo);
                if (cityInfoToClass.Results[0].Locations[0].LatLng.Lat == 39.78373 &&
                    cityInfoToClass.Results[0].Locations[0].LatLng.Lng == -100.445882)
                {
                    return null; //todo:написать про ошибку
                }
                return cityInfoToClass;
            }
        }
    }
}
