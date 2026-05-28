using System;
using System.Collections.Generic;
using System.Threading;

namespace Systems.Shared.Channels
{
    public interface IEventChannel
    {
        void Subscribe<TEvent>(Action<TEvent> handler, CancellationToken ct);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
    }

    public interface IEventChannelLogRules
    {
        List<EventLogRule> EventLogRules { get; }

        void RefreshLogRules();
        void SetAllRules(bool state);
    }
}
