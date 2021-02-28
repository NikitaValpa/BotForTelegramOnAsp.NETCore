using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using System.Net.Http;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using System.Net.Http.Headers;
using Telegram.Bot.Types.ReplyMarkups;
using BotForTelegram.Models;

namespace BotForTelegram.Controllers
{
    
    public class MessageController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public ContentResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)System.Net.HttpStatusCode.OK,
                Content = "<!DOCTYPE html>"+
                            "<html>"+
                                "<head>"+
                                    "<meta charset=\"utf-8\">" +
                                    "<title>Foul_bot</title>" +
                                "</head>"+
                                "<body>"+
                                    "<p> Вас приветствует foul_bot, для общения со мной перейдите по ссылке \n <a href=\"https://t.me/fl_language_bot\">t.me/fl_language_bot</a></p>" +
                                "</body>" +
                            "</html>"
            };
        }
        [HttpPost]
        [Route("api/message/update")]
        public async Task<OkResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            var commands = Bot.Commands;
            Message message = null;
            if (update.Message != null)
            {
                message = update.Message;
            }
            else if (update.CallbackQuery != null)
            {
                message = update.CallbackQuery.Message;
            }
            else
            {
                return Ok();
            }


            var botClient = await Bot.GetBotClientAsWebhookAsync();

            bool helpTrigger = true;
            string messageForClient = "";

            /*Итерируем комманды которые запихали в список при инициализации нашего бот клиента и запускаем 
             * ту комманду на выполнение, которая содержит в тот message которы пришёл к нам в метод через объект update*/
            if (message != null)
            {
                foreach (var command in commands)
                {
                    if (command.Contains(message))
                    {
                        await command.Execute(message, botClient);
                        helpTrigger = false;
                        break;
                    }
                }

                if (helpTrigger && message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    foreach (var item in Bot.Commands)
                    {
                        messageForClient += item.Name + " - " + item.Discription + "\n";
                    }
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                        text: "Я не понимаю, что вы от меня хотите, вот список поддерживаемых мною комманд: \n" + messageForClient,
                        replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
            return Ok();
        }
    }
}
