using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ChatFirst.Channels.BotFramework
{
    public class MessageLoggingHandler : MessageHandler
    {
        private static Logger _logger = LogManager.GetLogger("WebApiLogger");

        protected override Task IncomingMessageAsync(string correlationId, HttpMethod method, Uri uri, byte[] message)
        {
            return Task.Run(() =>
            {
                var log = $"{correlationId} - Request - Method:{method} Uri:'{uri}' Message:{Encoding.UTF8.GetString(message)}";
                _logger.Trace(log);
                Debug.WriteLine(log);
            });
        }

        protected override Task OutgoingMessageAsync(string correlationId, HttpMethod method, Uri uri, byte[] message)
        {
            return Task.Run(() =>
            {
                var log = $"{correlationId} - Response - Method:{method} Uri:'{uri}' Message:{Encoding.UTF8.GetString(message)}";
                _logger.Trace(log);
                Debug.WriteLine(log);
            });
        }
    }
}