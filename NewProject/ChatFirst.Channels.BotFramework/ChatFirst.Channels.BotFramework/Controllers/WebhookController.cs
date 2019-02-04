using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
//using NLog;

namespace ChatFirst.Channels.BotFramework.Controllers
{
    public class WebhookController : ApiController
    {
      //  private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     POST: api/v1/webhook/{userToken}/{botName}
        ///     Receive a message from a user and reply to it
        /// </summary>
        [HttpPost]
        [Route("api/v1/webhook/{userToken}/{botName}")]
        public async Task<IHttpActionResult> Post(string userToken, string botName, [FromBody] Activity activity)
        {
            try
            {
               // Trace.TraceInformation(
               //     $"[{userToken}:{botName}] Message from {activity.From.Id} to {activity.ServiceUrl}");
                if (activity.Type == "message")
                    await AnswerAsync(activity, default(CancellationToken));
                return Ok();
            }
            catch (Exception e)
            {
                //Trace.TraceError(e.ToString());
                throw;
            }
        }

        private static async Task AnswerAsync(Activity activity, CancellationToken ct)
        {
            try
            {
                var client = new ConnectorClient(new Uri(activity.ServiceUrl),
                    new MicrosoftAppCredentials("appid", @"apppassword"));
                var answer = activity.CreateReply(activity.Text, activity.Locale);
                MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);

                await client.Conversations.ReplyToActivityAsync(answer, ct);
            }
            catch (Exception e)
            {
               //Trace.TraceError(e.ToString());
                throw;
            }
        }

        private static async Task ShowTyping(Activity activity, ConnectorClient client)
        {
            var typingResponse = new Activity
            {
                Conversation = activity.Conversation,
                From = activity.Recipient,
                Locale = activity.Locale,
                Recipient = activity.From,
                ReplyToId = activity.Id,
                Id = activity.Id,
                Type = "typing"
            };
            await client.Conversations.UpdateActivityAsync(typingResponse);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Trace)
            {
            }

            return null;
        }
    }
}
