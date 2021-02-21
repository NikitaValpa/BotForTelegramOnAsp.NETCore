using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotForTelegram.Models.Commands
{
    /// <summary>
    /// Базовый класс для всех комманд отправляемых нащему боту
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Имя комманды
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание комманды
        /// </summary>
        public abstract string Discription { get; }

        /// <summary>
        /// Метод который непосредственно выполняет комманду
        /// </summary>
        /// <param name="message"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public abstract Task Execute(Message message, TelegramBotClient client);
        /// <summary>
        /// Метод который определяет вызвали комманду или нет
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool Contains(Message message);
    }
}
