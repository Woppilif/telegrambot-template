using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blink.Bot.Commands;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Blink.Bot
{
    internal class Program
    {
        private static ITelegramBotClient _botClient;
        private static string _botToken;
        private static Dictionary<string, Command> _commands;
        
        public static void Main(string[] args)
        {
            LoadConfigurtaion();
            
            _botClient = new TelegramBotClient(_botToken);

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            
            LoadCommands();
            
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            _botClient.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                ExecuteCommand(e.Message);
            }
        }
        
        private static void LoadConfigurtaion()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            _botToken = configuration["BotToken"] ?? "";
        }

        private static void LoadCommands()
        {
            _commands = new Dictionary<string, Command>();
            
            foreach (Type type in
                Assembly.GetAssembly(typeof(Command)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Command))))
            {
                var cmd = (Command)Activator.CreateInstance(type);
                _commands.Add(cmd.Name,cmd);
            }
        }

        private static void ExecuteCommand(Message message)
        {
            var comm = _commands
                .FirstOrDefault(cmd => cmd.Key == message.Text);
            comm.Value?.Execute(message, _botClient);
        }
    }
}