using Telegram.Bot;
using Telegram.Bot.Types;

namespace Blink.Bot.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract void Execute(Message msg, ITelegramBotClient client);
    }
}