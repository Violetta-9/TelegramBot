using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Telegram.Application.Contracts
{

    

        public class Temp
        {
            [JsonProperty("day")] public double Day { get; set; }
            [JsonProperty("min")] public double MinTemp { get; set; }
            [JsonProperty("max")] public double MaxTemp { get; set; }
            [JsonProperty("night")] public double Night { get; set; }
            [JsonProperty("eve")] public double Evening { get; set; }
            [JsonProperty("morn")] public double Morning { get; set; }
        }

        public class FeelsLike
        {
            [JsonProperty("day")] public double Day { get; set; }
            [JsonProperty("night")] public double Night { get; set; }
            [JsonProperty("eve")] public double Evening { get; set; }
            [JsonProperty("morn")] public double Morning { get; set; }
        }

        public class WeatherDescript
        {
            [JsonProperty("description")] public string Description { get; set; }
            [JsonProperty("icon")] public string Icon { get; set; }
        }

        public class Daily
        {
            [JsonConverter(typeof(CustomDateTimeConverter))]
            [JsonProperty("dt")]
            public DateTime Dt { get; set; }

            [JsonProperty("sunrise")] public int Sunrise { get; set; }
            [JsonProperty("sunset")] public int Sunset { get; set; }
            [JsonProperty("temp")] public Temp Temp { get; set; }
            [JsonProperty("feels_like")] public FeelsLike FeelsLike { get; set; }
            [JsonProperty("pressure")] public int Pressure { get; set; }
            [JsonProperty("humidity")] public int Humidity { get; set; }
            [JsonProperty("wind_speed")] public double WindSpeed { get; set; }
            public List<WeatherDescript> Weather { get; set; }
            [JsonProperty("clouds")] public int Clouds { get; set; }
            public double Pop { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public double Snow { get; set; } //объем осадков при наличии

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public double Rain { get; set; }

        }

        public class WeatherInfo
        {
            public List<Daily> Daily { get; set; }
        }

    }

    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime) value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            return _epoch.AddSeconds((long) reader.Value).ToLocalTime();
        }
    }


