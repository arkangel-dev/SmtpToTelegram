using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SmtpForwarder {
    public class TelegramHandler {
        private string BotToken = string.Empty;
        private string TargetChat = string.Empty;
        private TelegramBotClient? Bot = null;


        public TelegramHandler(string bottoken, string targetchat) {
            this.BotToken = bottoken;
            this.TargetChat = targetchat;
            this.Bot = new TelegramBotClient(this.BotToken);
        }

        public string GetSelfName() {
            return Bot?.GetMeAsync().Result.FirstName ?? "Unknown";
        }

        public void SendMessage(string message) {
            Bot?.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(TargetChat), message, ParseMode.Markdown).Wait();
        }



    }
}
