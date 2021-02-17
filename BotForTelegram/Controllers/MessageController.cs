using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BotForTelegram.Controllers
{
    [Route("api/message/update")]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Приложение работает и исправно ответило на гет запрос";
        }
        [HttpPost]
        public async Task<OkResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            var commands = Bot.Commands;
            var message = update.Message;
            var botClient = await Bot.GetBotClientAsync();

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
