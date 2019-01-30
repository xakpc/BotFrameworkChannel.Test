using System;

namespace ChatFirst.Channels.BotFramework.Models
{
    [Serializable]
    public class BotToken
    {
        public string UserToken { get; }

        public string BotName { get; }

        public BotToken(string userToken, string botName)
        {
            UserToken = userToken;
            BotName = botName;
        }

        public override bool Equals(object obj)
        {
            return Equals((BotToken)obj);
        }

        protected bool Equals(BotToken other)
        {
            return string.Equals(UserToken, other.UserToken, StringComparison.OrdinalIgnoreCase) && string.Equals(BotName, other.BotName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserToken != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(UserToken) : 0) * 397) ^ (BotName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(BotName) : 0);
            }
        }
    }
}