using System.Collections.Generic;

namespace Hippopotamus.Engine.Core.Messaging
{
    public delegate void MessageHandler<in TArg>(TArg arg);
    public static class MessageChannel<TMessage> where TMessage : class, IMessage
    {
        private static readonly List<TMessage> messages;
        private static readonly List<MessageHandler<TMessage>> handlers;

        static MessageChannel()
        {
            messages = new List<TMessage>();
            handlers = new List<MessageHandler<TMessage>>();
            MessageSystem.Subscribe(Invoke);
        }

        public static void Subscribe(MessageHandler<TMessage> handler)
        {
            handlers.Add(handler);
        }

        public static void Unsubscribe(MessageHandler<TMessage> handler)
        {
            handlers.Remove(handler);
        }

        public static void Broadcast(TMessage message)
        {
            messages.Add(message);
        }

        private static void Invoke()
        {
            int length = messages.Count;
            for (int i = 0; i < length; i++)
            {
                foreach (MessageHandler<TMessage> handler in handlers)
                {
                    handler(messages[i]);
                }
            }

            messages.RemoveRange(0, length);
        }
    }
}
