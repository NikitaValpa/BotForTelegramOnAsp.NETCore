using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotForTelegram.Controllers;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotForTelegram.Models.Commands
{
    public class AboutCommand : Command
    {
        public override string Name => @"/about";

        public override string Discription => "О боте";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(Name) || message.Text.Contains("О боте");
        }

        public async override Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Этот бот может выдавать вам погодные данные разных городов с помощью OpenWeatherAPI, для того чтобы посмотреть список комманд, наберите /help",
                replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
