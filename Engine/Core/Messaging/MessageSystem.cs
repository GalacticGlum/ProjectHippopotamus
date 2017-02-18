using System.Collections.Generic;

namespace Hippopotamus.Engine.Core.Messaging
{
    internal delegate void MessageHandler();
    internal static class MessageSystem
    {
        private static readonly List<MessageHandler> messages;
        static MessageSystem()
        {
            messages = new List<MessageHandler>();
        }

        public static void Subscribe(MessageHandler handler)
        {
            messages.Add(handler);
        }

        public static void Unsubscribe(MessageHandler handler)
        {
            messages.Add(handler);
        }

        internal static void Update()
        {
            foreach (MessageHandler message in messages)
            {
                message();
            }  
        }
    }
}
