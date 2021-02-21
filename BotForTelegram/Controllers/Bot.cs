using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using BotForTelegram.Models;
using BotForTelegram.Models.Commands;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Args;

namespace BotForTelegram.Controllers
{
    /// <summary>
    /// Этот класс инициализирует и запускает на прослушивание нашего бот клиента а также список комманд, которые мы ему можем отправить
    /// </summary>
    public static class Bot
    {

        public static TelegramBotClient botClient;
        private static List<Command> commandsList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsWebhookAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            commandsList.Add(new HelpCommand());
            //TODO: Add more commands

            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
