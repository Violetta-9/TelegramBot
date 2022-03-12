using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Telegram.Application.Contracts;

namespace Telegram.Application.Queries
{
   public  class GetWeather:IRequest<WeatherInfo>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GetWeather(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
