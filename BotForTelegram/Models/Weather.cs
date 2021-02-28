namespace BotForTelegram.Models
{
    public class Weather
    {
        /// <summary>
        /// id погодных условий
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Группа погодных параметров (Дождь, Снег, Экстрим и др.)
        /// </summary>
        public string main { get; set; }
        /// <summary>
        /// Погодные условия в группе. Вы можете получить вывод на своем языке.
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Идентификатор значка погоды
        /// </summary>
        public string icon { get; set; }
    }
}