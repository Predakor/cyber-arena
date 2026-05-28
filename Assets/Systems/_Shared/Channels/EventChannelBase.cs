using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Systems.Shared.Channels
{
    public abstract class EventChannelBase<TChild> : ScriptableObject, IEventChannel, IEventChannelLogRules
        where TChild : EventChannelBase<TChild>
    {
        protected const string MenuName = "Channels/";

        [Header("Event Log Filtering (by container type name)")]
        [SerializeField] protected EventChannellLoger<TChild> _logger;
        [SerializeField] protected List<EventLogRule> _rules;

        private readonly Dictionary<Type, Delegate> _handlers = new();
        public List<EventLogRule> EventLogRules => _rules;

        public void RefreshLogRules() => _logger.SyncEventLogRules();
        public void SetAllRules(bool state) => _logger.SetAllRules(state);

        private void OnEnable()
        {
            _logger = new EventChannellLoger<TChild>(this, _rules);
        }
        private void OnValidate() => _logger.ClearCache();

        /// <summary>Subscribe with gameobject.destroyCancelationToken</summary>
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

            if (_logger.IsEventLoggingEnabled(eventType))
            {
                var fullHandlerName = $"{handler.Target?.GetType().Name}.{handler.Method.Name}";
                _logger.LogEvent($"<b><color=green>[O]</color></b>: {eventType.Name} by {fullHandlerName}");
            }

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

            if (_logger.IsEventLoggingEnabled(eventType))
            {
                var fullHandlerName = $"{handler.Target?.GetType().Name}.{handler.Method.Name}";
                _logger.LogEvent($"<b><color=red>[O]--</color></b>: {eventType.Name} by {fullHandlerName}");
            }
        }

        public void Raise<TEvent>(TEvent evt)
        {
            Type eventType = typeof(TEvent);

            if (_logger.IsEventLoggingEnabled(eventType))
            {
                _logger.LogEvent($"<b><color=yellow>[EVENT]>></color></b>: {eventType.Name}: {evt}");
            }

            if (_handlers.TryGetValue(eventType, out var del) && del is Action<TEvent> action)
            {
                action.Invoke(evt);
            }
        }
    }
}
