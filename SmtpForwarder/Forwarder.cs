using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SmtpForwarder {
    public class Forwarder {

        public delegate void StringDelegate(string s);

        public event StringDelegate OnDebug;

        private SMTPServer? smtpServer;
        private TelegramHandler? telegramHandler;
        public void Start() {

            var configReader = GetConfigReader();

            var hostname = configReader.GetConfigValue("hostname").StringValue;
            var port = configReader.GetConfigValue("port").IntegerValue;
            var bottoken = configReader.GetConfigValue("bot_token").StringValue;
            var targetchat = configReader.GetConfigValue("target_chat").StringValue;

            OnDebug?.Invoke($"Starting SMTP server on {hostname}:{port}...");
            smtpServer = new SmtpForwarder.SMTPServer(hostname, port);
            OnDebug?.Invoke("Awakening the bot...");
            telegramHandler = new TelegramHandler(bottoken, targetchat);
            OnDebug?.Invoke($"Bot identified as {telegramHandler.GetSelfName()}...\nBeep boop...");
            smtpServer.OnDebugLog += DebugMessage;
            smtpServer.OnNewEmail += SendMessage;
            smtpServer.Build();
        }

        private void DebugMessage(string message) => Console.WriteLine(message);

        private void SendMessage(MimeMessage message) {
            OnDebug?.Invoke("New message... Forwarding...");
            new Task(() => {

                telegramHandler?.SendMessage(HtmlToPlainText(message.GetTextBody(MimeKit.Text.TextFormat.Html)));
            }).Start();
        }

        public void Restart() {
            if (smtpServer != null) {
                smtpServer.OnNewEmail -= SendMessage;
                smtpServer.OnDebugLog -= DebugMessage;
                smtpServer.server?.ShutdownTask.ContinueWith((Task t) => {
                    Start();
                });
                smtpServer?.Shutdown();
                //smtpServer = null;
                //telegramHandler = null;
                if (smtpServer.server == null) {
                    Start();
                }
            } else {
                Start();
            }
            
        }

        // Stolled from https://stackoverflow.com/a/16407272
        private static string HtmlToPlainText(string html) {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br/> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public FixedConfigReader GetConfigReader() {
            return new FixedConfigReader(
                JObject.Parse(System.IO.File.ReadAllText("config.json")),
                (FixedConfigReader f) => {
                    System.IO.File.WriteAllText("config.json", f.DataSource.ToString(Newtonsoft.Json.Formatting.Indented));
                },
                new FixedConfigReader.FixedConfigReaderSegment("localhost", "hostname"),
                new FixedConfigReader.FixedConfigReaderSegment(5025, "port"),
                new FixedConfigReader.FixedConfigReaderSegment("??", "bot_token"),
                new FixedConfigReader.FixedConfigReaderSegment("??", "target_chat")
            );
        }
    }
}
