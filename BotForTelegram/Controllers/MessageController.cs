using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;

namespace BotForTelegram.Controllers
{
    
    public class MessageController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public string Get()
        {
            return "Вас приветствует foul_bot, для общения со мной перейдите по ссылке \n" +
                "<a href=\"t.me/fl_language_bot\">t.me/fl_language_bot</a>";
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
        /// <summary>
        /// Этот метод создан для дебаггинга на компьютере разработчика и вызывается только в случае запуска на консоли
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ExecutingCommandsForDebuging(object sender, MessageEventArgs e) {
            if (e.Message.Text != null) {
                var commands = Bot.Commands;
                var message = e.Message;

                foreach (var command in commands) {
                    if (command.Contains(message))
                    {
                        if (Bot.botClient != null)
                        {
                            await command.Execute(message, Bot.botClient);
                        }
                        break;
                    }
                }


            }
        }
    }
}
