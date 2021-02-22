using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using BotForTelegram.Controllers;
using BotForTelegram.EmojiList;

namespace BotForTelegram.Models
{
    /// <summary>
    /// Класс с массивом для виртуальной клавиатуры
    /// </summary>
    public static class KeyBoards
    {
        /// <summary>
        /// Массив с клавишами для виртуальной клавиатуры
        /// </summary>
        static public readonly KeyboardButton[] keyboardButtons = { 
            new KeyboardButton { Text = Emoji.Grey_Question + " Помощь" },
            new KeyboardButton { Text = Emoji.Moyai + " О боте" } 
        };
    }
}
