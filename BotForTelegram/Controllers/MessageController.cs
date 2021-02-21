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
            var message = update.Message;
            var botClient = await Bot.GetBotClientAsWebhookAsync();

            /*Итерируем комманды которые запихали в список при инициализации нашего бот клиента и запускаем 
             * ту комманду на выполнение, которая содержит в тот message которы пришёл к нам в метод через объект update*/
            foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
                    break;
                }
            }
            return Ok();
        }
    }
}
