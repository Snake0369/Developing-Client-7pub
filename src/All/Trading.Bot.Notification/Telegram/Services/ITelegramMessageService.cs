using System.Threading.Tasks;

namespace Trading.Bot.Notification.Telegram.Services
{
    public interface ITelegramMessageService
    {
        Task ToConcatedString(params object[] toConcatenate);
    }
}
