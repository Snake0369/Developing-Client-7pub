using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Trading.Bot.Notification.Telegram.Services
{
    public class TelegramMessageService : ITelegramMessageService
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(TelegramMessageService));
        private readonly TelegramBotClient _telegramClient;

        public TelegramMessageService()
        {
            string tgBotToken = "1644502757:AAHvD9ogka9kGyz6555jVAHccQ3QueiXnwY";
            _telegramClient = new TelegramBotClient(tgBotToken);
        }

        private async Task SendMessageToChannel(string message)
        {
            try
            {
                string tgChatId = "-1001620573315";
                await _telegramClient.SendTextMessageAsync(tgChatId, message, disableWebPagePreview: true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"SendMessageToChanel:\n {message}");
            }
        }

        public async Task ToConcatedString(params object[] toConcatenate)
        {
            await SendMessageToChannel($"\U00002139  " + string.Concat(toConcatenate));
        }
    }
}
