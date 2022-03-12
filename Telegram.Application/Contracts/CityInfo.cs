using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Application.Contracts
{
   
        public class ProvidedLocation
        {
            [JsonProperty("location")]
            public string Location { get; set; }
        }

        public class LatLng
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }


        public class Location
        {
            [JsonProperty("adminArea6Type")]
            public string CityName { get; set; }

            public LatLng LatLng { get; set; }

            public string MapUrl { get; set; }
        }

        public class Result
        {
            public ProvidedLocation ProvidedLocation { get; set; }
            public List<Location> Locations { get; set; }
        }

        public class CityInfo
        {

            public List<Result> Results { get; set; }
        }
    
}
