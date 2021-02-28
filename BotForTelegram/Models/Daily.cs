using System;
using System.Collections.Generic;

namespace BotForTelegram.Models
{
    public class Daily
    {
        /// <summary>
        /// время в unix формате
        /// </summary>
        public int dt { get; set; }

        public DateTime date => new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dt);
        /// <summary>
        /// данные по температурам
        /// </summary>
        public Temp temp { get; set; }
        /// <summary>
        /// данные по температурам на ощущения
        /// </summary>
        public Feels_like feels_like { get; set; }
        /// <summary>
        /// давление в гПа
        /// </summary>
        public int pressure { get; set; }
        /// <summary>
        /// влажность в %
        /// </summary>
        public int humidity { get; set; }
        /// <summary>
        /// скорость ветра в м/с
        /// </summary>
        public double wind_speed { get; set; }
        /// <summary>
        /// направление ветра в градусах
        /// </summary>
        public int wind_deg { get; set; }
        /// <summary>
        /// объект с описанием погодных условий
        /// </summary>
        public List<Weather> weather { get; set; }
        /// <summary>
        /// облачность
        /// </summary>
        public int clouds { get; set; }
    }
}