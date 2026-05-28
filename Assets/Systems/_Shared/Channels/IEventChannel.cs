using System;

namespace Systems.Shared.Channels
{
    public interface IEventChannel
    {
        void Subscribe<TEvent>(Action<TEvent> handler);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
    }
}
