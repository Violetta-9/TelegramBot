using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Application.Contracts
{
    public class Settings
    {
        public string CityToken { get; set; }
        public string WeatherToken { get; set; }
        public string AdminId { get; set; }
    }
}
