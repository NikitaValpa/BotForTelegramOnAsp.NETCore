namespace BotForTelegram.Models
{
    public class Feels_like
    {
        /// <summary>
        /// Температура по ощущениям днём
        /// </summary>
        public double day { get; set; }
        /// <summary>
        /// Температура по ощущениям ночью
        /// </summary>
        public double night { get; set; }
        /// <summary>
        /// Температура по ощущениям вечером
        /// </summary>
        public double eve { get; set; }
        /// <summary>
        /// Температура по ощущениям утром
        /// </summary>
        public double morn { get; set; }
    }
}