using System.Net;
// Подключаем библиотеку "Telegram.Bot" https://github.com/TelegramBots/telegram.bot
using Telegram.Bot;

namespace LauncherBot
{
    
    class Program
    {
        static string TOKEN = "TOKEN";
        // Создаем объекты класса Телеграм бота
        private static TelegramBotClient Client;
        // Создаем объект класса прокси для теста бота на территории где провайдеры блокируют подключение к серверу телеграм
        private static WebProxy ProxyClient;
        // Точка входа в консольное приложение
        static void Main(string[] args)
        {         
            ProxyClient = new WebProxy(GetProxy.ParseProxies(), true);
            Client = new TelegramBotClient(TOKEN, ProxyClient);
            Client.OnMessage += BotOnMessage;
            Client.OnInlineQuery += BotInline;
        }
        private static void BotOnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

        }
        private static void BotInline(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {

        }
    }
}
