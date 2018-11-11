using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace LauncherBot
{
    
    class Program
    {
        static string TOKEN = "730658494:AAFCWkKo491owHwlPO54-RrPoTI6tFnHH0w";
        private static WebProxy ProxyClient = new WebProxy(GetProxy.ParseProxies("Iran"));
        // Создаем объекты класса Телеграм бота
        private static TelegramBotClient Bot = new TelegramBotClient(TOKEN, ProxyClient);
        // Точка входа в консольное приложение
        public static void Main(string[] args)
        {
            try
            {
                var me = Bot.GetMeAsync().Result;

            
            
            Console.Title = me.Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            Bot.StopReceiving();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text) return;

            switch (message.Text.Split(' ').First())
            {
                // send inline keyboard
                case "/menu":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("Правила"),
                            InlineKeyboardButton.WithCallbackData("Библиотека"),
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithCallbackData("Репозитории"),
                            InlineKeyboardButton.WithCallbackData("Статистика"),
                        }
                    });

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Меню чата:",
                        replyMarkup: inlineKeyboard);
                    break;

                // send custom keyboard
                case "/help":
                    ReplyKeyboardMarkup ReplyKeyboard = new[]
                    {
                        new[] { "1.1", "1.2" },
                        new[] { "2.1", "2.2" },
                    };

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Меню:",
                        replyMarkup: ReplyKeyboard);
                    break;

                // send a photo
                case "/photo":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                    const string file = @"/bicycle.png";

                    var fileName = file.Split(Path.DirectorySeparatorChar).Last();

                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await Bot.SendPhotoAsync(
                            message.Chat.Id,
                            fileStream,
                            "Nice Picture");
                    }
                    break;

                // request location or contact
                //case "/request":
                //    var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                //    {
                //        KeyboardButton.WithRequestLocation("Location"),
                //        KeyboardButton.WithRequestContact("Contact"),
                //    });

                //    await Bot.SendTextMessageAsync(
                //        message.Chat.Id,
                //        "Who or Where are you?",
                //        replyMarkup: RequestReplyKeyboard);
                //    break;

                //case "/help":
                //    const string usage = @"
                //    Команды:
                //    /menu   - меню чата";
                //    /// - send custom keyboard
                //    ///photo    - send a photo
                //    ///request  - request location or contact";

                //    await Bot.SendTextMessageAsync(
                //        message.Chat.Id,
                //        usage,
                //        replyMarkup: new ReplyKeyboardRemove());
                //    break;
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            if (callbackQuery.Message.Text == "Правила")
            {
                await Bot.AnswerCallbackQueryAsync(
                    callbackQuery.Id, @"Правила чата:
1. Не оскарбляй учатсников !
");
            }


            await Bot.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                @"Правила чата:
1. Не оскарбляй учатсников !
");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                new InlineQueryResultLocation(
                    id: "1",
                    latitude: 40.7058316f,
                    longitude: -74.2581888f,
                    title: "New York")   // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 40.7058316f,
                            longitude: -74.2581888f)    // message if result is selected
                    },

                new InlineQueryResultLocation(
                    id: "2",
                    latitude: 13.1449577f,
                    longitude: 52.507629f,
                    title: "Berlin") // displayed result
                    {

                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 13.1449577f,
                            longitude: 52.507629f)   // message if result is selected
                    }
            };

            await Bot.AnswerInlineQueryAsync(
                inlineQueryEventArgs.InlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }
    }
}
