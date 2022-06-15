/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using System;

namespace WebLocalizerBlazorSrvDemo.Data
{
    public class WeatherForecast
    {
        public Guid Id { get; internal set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}