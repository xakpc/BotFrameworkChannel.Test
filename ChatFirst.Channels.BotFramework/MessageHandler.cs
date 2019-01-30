using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatFirst.Channels.BotFramework
{
    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var corrId = $"{DateTime.Now.Ticks}{Thread.CurrentThread.ManagedThreadId}";

            var requestMessage = await request.Content.ReadAsByteArrayAsync();

            await IncomingMessageAsync(corrId, request.Method, request.RequestUri, requestMessage);

            var response = await base.SendAsync(request, cancellationToken);

            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
                responseMessage = response.Content != null
                    ? await response.Content.ReadAsByteArrayAsync()
                    : Encoding.UTF8.GetBytes("null");
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            await OutgoingMessageAsync(corrId, request.Method, request.RequestUri, responseMessage);

            return response;
        }

        protected abstract Task IncomingMessageAsync(string correlationId, HttpMethod method, Uri uri, byte[] message);
        protected abstract Task OutgoingMessageAsync(string correlationId, HttpMethod method, Uri uri, byte[] message);
    }
}