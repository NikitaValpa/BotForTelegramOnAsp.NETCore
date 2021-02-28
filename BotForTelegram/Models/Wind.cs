namespace BotForTelegram.Models
{
    public class Wind
    {
        /// <summary>
        /// Скорость ветра. Единица измерения по умолчанию: метр / сек, метрическая система: метр / сек, британская система мер: мили / час.
        /// </summary>
        public double speed { get; set; }
        /// <summary>
        /// Направление ветра, градусы (метеорологические)
        /// </summary>
        public int deg { get; set; }
    }
}