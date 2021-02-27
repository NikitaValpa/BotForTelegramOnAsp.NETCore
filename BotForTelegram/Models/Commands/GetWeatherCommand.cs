using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace BotForTelegram.Models.Commands
{
    public class GetWeatherCommand : Command
    {
        public override string Name => @"/weather";

        public override string Discription => "Запрос погоды, пример: /weather Москва";

        private Dictionary<string, string> queryparams = new Dictionary<string, string> {
            ["city"] = "",
            ["units"] = "metric",
            ["lang"] = "ru",
        };

        

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            if (message.Text.Contains(this.Name))
            {
                var ParsingParams = message.Text.Split(" ");
                if (ParsingParams.Length == 2)
                {
                    queryparams["city"] = ParsingParams[1]; // первый элемент в парсингпарамс массиве, по идее это название города
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
            
        }
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new SerilogLoggerProvider());
            });
            ILogger<GetWeatherCommand> logger = loggerFactory.CreateLogger<GetWeatherCommand>();//создаём типизированный логгер

            HttpClient client = new HttpClient();

            logger.LogInformation("Делаю запрос к OpenWeatherAPI " + String.Format(AppSettings.UrlForOpenWeatherApiCurrentWeather,
                                                                $"q={queryparams["city"]}&units={queryparams["units"]}&lang={queryparams["lang"]}",
                                                                AppSettings.keyForOpenWeather));
            string response = null;
            try
            {
                response = await client.GetStringAsync(String.Format(AppSettings.UrlForOpenWeatherApiCurrentWeather,
                                                                $"q={queryparams["city"]}&units={queryparams["units"]}&lang={queryparams["lang"]}",
                                                                AppSettings.keyForOpenWeather));
            }
            catch (Exception ex)
            {
                logger.LogError("При запросе к OpenWeatherAPI произошло исключение " + ex.Message);
                var chatId = message.Chat.Id;
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Город, который вы ввели не существует, либо не удаётся получить данные о погоде в этом городе",
                    replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }

            if (response != null) {
                logger.LogInformation("Получил ответ: " + response);
            }

            client.Dispose();

        }
    }
}
