namespace BotForTelegram.Models
{
    public class Main
    {
        /// <summary>
        /// Температура. Единица измерения по умолчанию: Кельвин, Метрическая система: Цельсий, Имперская система: Фаренгейт.
        /// </summary>
        public double temp { get; set; }
        /// <summary>
        /// Температура. Этот температурный параметр объясняет человеческое восприятие погоды. Единица измерения по умолчанию: Кельвин, Метрическая система: Цельсий, Имперская система: Фаренгейт.
        /// </summary>
        public double feels_like { get; set; }
        /// <summary>
        /// Минимальная температура на данный момент.
        /// </summary>
        public double temp_min { get; set; }
        /// <summary>
        /// Максимальная температура на данный момент.
        /// </summary>
        public double temp_max { get; set; }
        /// <summary>
        /// Атмосферное давление (на уровне моря, если нет данных sea_level или grnd_level), гПа
        /// </summary>
        public int pressure { get; set; }
        /// <summary>
        /// Влажность, %
        /// </summary>
        public int humidity { get; set; }
    }
}