using MimeKit;
using Newtonsoft.Json.Linq;
using SmtpServer;
using SmtpServer.Authentication;
using SmtpServer.ComponentModel;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System.Buffers;
using System.Reflection.Metadata.Ecma335;

namespace SmtpForwarder {
    public class SMTPServer {
        public delegate void StringDelegate(string message);
        public delegate void MimeDelegate(MimeMessage message);
        public event StringDelegate? OnDebugLog;
        public event MimeDelegate? OnNewEmail;
        private int port;
        private string hostname;
        public SmtpServer.SmtpServer server;
        public SMTPServer(string hs, int port) {
            this.port = port;
            this.hostname = hs;
        }

        private CancellationTokenSource CancellationTokenSource;

        public async void Build() {
            var options = new SmtpServerOptionsBuilder()
                .ServerName(hostname)
                .Port(port)
                .Build();

            var serviceProvider = new ServiceProvider();
            serviceProvider.Add(new CustomMessageStore(this));
            serviceProvider.Add(new CustomUserAuthenticator());

            server = new SmtpServer.SmtpServer(options, serviceProvider);
            CancellationTokenSource = new CancellationTokenSource();
            
            new Task(async () => await server.StartAsync(CancellationTokenSource.Token)).Start();

        }

        public void Shutdown() {
            CancellationTokenSource?.Cancel();
        }

     

        public class CustomMessageStore : MessageStore {
            private SMTPServer Parent;
            public CustomMessageStore(SMTPServer s) {
                this.Parent = s;
            } 
            public override async Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken) {
                await using var stream = new MemoryStream();

                var position = buffer.GetPosition(0);
                while (buffer.TryGet(ref position, out var memory)) {
                    await stream.WriteAsync(memory, cancellationToken);
                }

                stream.Position = 0;

                var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);
                this.Parent.OnNewEmail?.Invoke(message);
                return SmtpResponse.Ok; 
            }
        }

        public class CustomUserAuthenticator : IUserAuthenticator {
            public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken cancellationToken) {
                return new Task<bool>(() => true);
            }
        }
    }
}