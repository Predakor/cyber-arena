using System;
using System.Collections.Generic;
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

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (_handlers.TryGetValue(typeof(TEvent), out var del))
            {
                _handlers[typeof(TEvent)] = Delegate.Combine(del, handler);
            }
            else
            {
                _handlers[typeof(TEvent)] = handler;
            }

            _logger.LogEvent(typeof(TEvent), $"+ {typeof(TEvent).Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (_handlers.TryGetValue(typeof(TEvent), out var del))
            {
                var current = Delegate.Remove(del, handler);
                if (current == null)
                {
                    _handlers.Remove(typeof(TEvent));
                }
                else
                {
                    _handlers[typeof(TEvent)] = current;
                }
            }

            _logger.LogEvent(typeof(TEvent), $"- {typeof(TEvent).Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
        }

        public void Raise<TEvent>(TEvent evt)
        {
            _logger.LogEvent(typeof(TEvent), $"▶ {typeof(TEvent).Name}: {evt}");

            if (_handlers.TryGetValue(typeof(TEvent), out var del) && del is Action<TEvent> action)
            {
                action.Invoke(evt);
            }
        }

    }
}
