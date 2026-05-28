using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Systems.Shared.Channels
{
    public abstract class EventChannelBase<TChild> : ScriptableObject, IEventChannel
        where TChild : EventChannelBase<TChild>
    {
        protected const string MenuName = "Channels/";

        [Header("Event Log Filtering (by container type name)")]
        [SerializeField] protected List<EventLogRule> _eventLogRules = new();
        [SerializeField] protected EventChannellLoger<TChild> _logger;

        private readonly Dictionary<Type, Delegate> _handlers = new();

        private void OnEnable()
        {
            _logger = new EventChannellLoger<TChild>(this, _eventLogRules);
            _logger.EnsureEventLogMap();
        }

        [Obsolete("This method does not support automatic unsubscription and may lead to memory leaks if not used carefully.")]
        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            Type eventType = typeof(TEvent);
            if (_handlers.TryGetValue(eventType, out var del))
            {
                _handlers[eventType] = Delegate.Combine(del, handler);
            }
            else
            {
                _handlers[eventType] = handler;
            }

            _logger.LogEvent(eventType, $"+ {eventType.Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
        }

        public void Subscribe<TEvent>(Action<TEvent> handler, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            Type eventType = typeof(TEvent);
            if (_handlers.TryGetValue(eventType, out var del))
            {
                _handlers[eventType] = Delegate.Combine(del, handler);
            }
            else
            {
                _handlers[eventType] = handler;
            }

            _logger.LogEvent(eventType, $"+ {eventType.Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");

            ct.Register(() => Unsubscribe(handler));
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            Type eventType = typeof(TEvent);
            if (_handlers.TryGetValue(eventType, out var del))
            {
                var current = Delegate.Remove(del, handler);
                if (current == null)
                {
                    _handlers.Remove(eventType);
                }
                else
                {
                    _handlers[eventType] = current;
                }
            }

            _logger.LogEvent(eventType, $"- {eventType.Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
        }

        public void Raise<TEvent>(TEvent evt)
        {
            Type eventType = typeof(TEvent);
            _logger.LogEvent(eventType, $"▶ {eventType.Name}: {evt}");

            if (_handlers.TryGetValue(eventType, out var del) && del is Action<TEvent> action)
            {
                action.Invoke(evt);
            }
        }

    }
}
