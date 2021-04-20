using Telegram.Bot;
using Telegram.Bot.Types;

namespace Blink.Bot.Commands.Common
{
    public class StartCommand : Command
    {
        public override string Name => "/start";
        
        public override async void Execute(Message msg, ITelegramBotClient client)
        {
            await client.SendTextMessageAsync(chatId: msg.Chat.Id, "Hello, there!");
        }
    }
}