namespace BotForTelegram.Models
{
    public class Temp
    {
        /// <summary>
        /// Температура днём
        /// </summary>
        public double day { get; set; }
        /// <summary>
        /// Мин температура
        /// </summary>
        public double min { get; set; }
        /// <summary>
        /// Макс температура
        /// </summary>
        public double max { get; set; }
        /// <summary>
        /// Температура ночью
        /// </summary>
        public double night { get; set; }
        /// <summary>
        /// Вечерняя температура
        /// </summary>
        public double eve { get; set; }
        /// <summary>
        /// Утреняя температура
        /// </summary>
        public double morn { get; set; }
    }
}