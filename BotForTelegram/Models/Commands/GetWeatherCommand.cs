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
using System.Globalization;
using System.Threading;

namespace BotForTelegram.Models.Commands
{
    public class GetWeatherCommand : Command
    {
        public override string Name => @"/weather";

        public override string Discription => "Запрос актуальной погоды, пример:\n /weather Москва или /погода Москва";

        private Dictionary<string, string> queryparams = new Dictionary<string, string> {
            ["city"] = "",
            ["units"] = "metric",
            ["lang"] = "ru",
            ["exclude"] = "current,minutely,hourly,alerts"
        };
        private string callbackdata { get; set; } = "";

        

        public override bool Contains(Message message)
        {        
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            if (message.ReplyMarkup != null)
            {
                if (message.ReplyMarkup.InlineKeyboard.Any())
                {
                    foreach (var InlineArray in message.ReplyMarkup.InlineKeyboard) {
                        foreach (var item in InlineArray) {
                            if (item.CallbackData.Contains("lat="))
                            {
                                callbackdata = item.CallbackData;
                            }
                            else {
                                return false;
                            }
                        }
                    }
                return true;
                }
                else
                return false;
            }
            else {
                if (message.Text.Contains(this.Name) || message.Text.Contains("/погода"))
                {
                    var ParsingParams = message.Text.Split(" ");
                    if (ParsingParams.Length == 2)
                    {
                        queryparams["city"] = ParsingParams[1]; // первый элемент в парсингпарамс массиве, это название города
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }    
        }
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new SerilogLoggerProvider());
            });
            ILogger<GetWeatherCommand> logger = loggerFactory.CreateLogger<GetWeatherCommand>();//создаём типизированный логгер
            var chatId = message.Chat.Id;


            HttpClient client = new HttpClient();

            if (callbackdata == "")//если в нашем коллбеке ничего нет, то мы выполняем запрос текущей погоды
            {
                logger.LogInformation("Делаю запрос к OpenWeatherAPI " + String.Format(AppSettings.UrlForOpenWeatherApiCurrentWeather,
                                                               $"q={queryparams["city"]}&units={queryparams["units"]}&lang={queryparams["lang"]}",
                                                               AppSettings.keyForOpenWeather));
                string response = null;
                try
                {
                    response = await client.GetStringAsync(String.Format(AppSettings.UrlForOpenWeatherApiCurrentWeather,
                                                                    $"q={queryparams["city"]}&units={queryparams["units"]}&lang={queryparams["lang"]}",
                                                                    AppSettings.keyForOpenWeather));

                    CurrentWeather deserializeResponse = JsonSerializer.Deserialize<CurrentWeather>(response);
                    
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"Город: {deserializeResponse.name} {EmojiList.Emoji.City_Sunrise}\n" +
                                $"Погода: {deserializeResponse.weather[0].main}, ({deserializeResponse.weather[0].description})\n" +
                                $"Температура: {deserializeResponse.main.temp}℃, \nмин({deserializeResponse.main.temp_min}℃) макс({deserializeResponse.main.temp_max}℃)\nпо ощущениям: {deserializeResponse.main.feels_like}℃\n" +
                                $"Давление: {deserializeResponse.main.pressure} гПа\n" +
                                $"Влажность: {deserializeResponse.main.humidity} %\n" +
                                $"Ветер: скорость ветра - {deserializeResponse.wind.speed}м/с, направление {deserializeResponse.wind.deg}°\n" +
                                $"Облачность: {deserializeResponse.clouds.all} %",
                        replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Прогноз на 7 дней", CallbackData = $"lat={deserializeResponse.coord.lat.ToString().Replace(",",".")}&lon={deserializeResponse.coord.lon.ToString().Replace(",", ".")}"}),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                catch (Exception ex)
                {
                    logger.LogError("При запросе к OpenWeatherAPI произошло исключение " + ex.Message);
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Город, который вы ввели не существует, либо не удаётся получить данные о погоде в этом городе",
                    replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }

                if (response != null)
                {
                    logger.LogInformation("Получил ответ: " + response);
                }
            }
            else {//иначе выполняем запрос погоды на 7 дней
                logger.LogInformation("Делаю запрос к OpenWeatherAPI " + String.Format(AppSettings.UrlForOpenWeatherApi7days,callbackdata+
                                                                                        $"&units={queryparams["units"]}&exclude={queryparams["exclude"]}&lang={queryparams["lang"]}"
                                                                                        ,AppSettings.keyForOpenWeather));
                string responseFrom7days = null;
                try
                {
                    responseFrom7days = await client.GetStringAsync(String.Format(AppSettings.UrlForOpenWeatherApi7days, callbackdata +
                                                                                        $"&units={queryparams["units"]}&exclude={queryparams["exclude"]}&lang={queryparams["lang"]}",
                                                                                        AppSettings.keyForOpenWeather));

                    DailyWeather deserializeResponse = JsonSerializer.Deserialize<DailyWeather>(responseFrom7days);

                    foreach (var dayRes in deserializeResponse.daily) {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"Дата: {dayRes.date.Date.ToString("d")}\n" +
                                $"День: {CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(dayRes.date.DayOfWeek)}\n"+
                                $"Погода: {dayRes.weather[0].main}, ({dayRes.weather[0].description})\n" +
                                $"Температура:\n {EmojiList.Emoji.Sunrise} утром {dayRes.temp.morn}℃\n {EmojiList.Emoji.Sunny} днём {dayRes.temp.day}℃\n {EmojiList.Emoji.Sunrise_Over_Mountains} вечером {dayRes.temp.eve}℃\n {EmojiList.Emoji.Full_Moon} ночью {dayRes.temp.night}℃\n мин({dayRes.temp.min}℃) макс({dayRes.temp.max}℃)\n" +
                                $"Температура по ощущениям:\n {EmojiList.Emoji.Sunrise} утром {dayRes.feels_like.morn}℃\n {EmojiList.Emoji.Sunny} днём {dayRes.feels_like.day}℃\n {EmojiList.Emoji.Sunrise_Over_Mountains} вечером {dayRes.feels_like.eve}℃\n {EmojiList.Emoji.Full_Moon} ночью {dayRes.feels_like.night}℃\n" +
                                $"Давление: {dayRes.pressure} гПа\n" +
                                $"Влажность: {dayRes.humidity} %\n" +
                                $"Ветер: скорость ветра - {dayRes.wind_speed}м/с, направление {dayRes.wind_deg}°\n" +
                                $"Облачность: {dayRes.clouds} %",
                        replyMarkup: new ReplyKeyboardMarkup(KeyBoards.keyboardButtons, resizeKeyboard: true),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("При запросе к OpenWeatherAPI произошло исключение " + ex.Message);
                }

                if (responseFrom7days != null)
                {
                    logger.LogInformation("Получил ответ: " + responseFrom7days);
                }
                callbackdata = "";
            }
            client.Dispose();

        }
    }
}
