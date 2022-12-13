using System.Threading.Tasks;

namespace Trading.Notification.Telegram.Services
{
    public interface ITelegramMessageService
    {
        Task ToConcatedString(string tgChatId, params object[] toConcatenate);
    }
}
