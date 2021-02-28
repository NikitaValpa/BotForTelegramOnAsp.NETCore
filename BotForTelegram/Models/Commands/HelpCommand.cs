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
    public class HelpCommand : Command
    {
        public override string Name => @"/help";

        public override string Discription => "Комманда для вывода списка комманд";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(Name) || message.Text.Contains("Помощь");
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            string messageForClient = "";
            foreach (var command in Bot.Commands)
            {
                if (command.Name == Name) {
                    continue;
                }
                messageForClient += command.Name + " - " + command.Discription + "\n";
            }
            await botClient.SendTextMessageAsync(chatId: chatId, 
                text: messageForClient, 
                replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
