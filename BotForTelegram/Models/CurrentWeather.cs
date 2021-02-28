using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotForTelegram.Models
{
    public class CurrentWeather
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }

        public Main main { get; set; }

        public Wind wind { get; set; }

        public Clouds clouds { get; set; }
        /// <summary>
        /// ID города
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Название города
        /// </summary>
        public string name { get; set; }
    }
}
