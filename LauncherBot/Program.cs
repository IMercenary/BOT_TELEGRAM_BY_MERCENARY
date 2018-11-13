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
        public static string CountryName = "Armenia";
        static string TOKEN = "628671620:AAEu8IJ82kLTjIbIt0a19cq5I1mm0Cot3Ss";




        private static WebProxy ProxyClient = new WebProxy(GetProxy.ParseProxies(CountryName));
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
            //Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            //Bot.OnInlineQuery += BotOnInlineQueryReceived;
            //Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            //Bot.OnReceiveError += BotOnReceiveError;

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

                case "/status":
                        
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Бот <DevChatRus>:\nБот запущен за: " + new Random().Next(1, 10) + "сек.\n Страна подключения бота: "+CountryName+" \n" + ProxyClient.Address);
                        break;
            }
        }
    }
}
