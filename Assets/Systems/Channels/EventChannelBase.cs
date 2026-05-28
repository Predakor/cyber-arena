using System;
using System.Collections.Generic;
using Systems.Shared.Loggers;
using UnityEngine;

namespace Systems.Channels
{
    public interface IEventChannel
    {
        void Subscribe<TEvent>(Action<TEvent> handler);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
    }

    public abstract class EventChannelBase<TChild> : ScriptableObject, IEventChannel
        where TChild : EventChannelBase<TChild>
    {
        protected const string MenuName = "Channels/";

        private readonly Dictionary<Type, Delegate> _handlers = new();
        private LogHandler<TChild> _logger;

        [SerializeField] private bool _debugMode = false;

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

            Log($"+ {typeof(TEvent).Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
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

            Log($"- {typeof(TEvent).Name} ← {handler.Target?.GetType().Name}.{handler.Method.Name}");
        }

        public void Raise<TEvent>(TEvent evt)
        {
            Log($"▶ {typeof(TEvent).Name}: {evt}");

            if (_handlers.TryGetValue(typeof(TEvent), out var del) && del is Action<TEvent> action)
            {
                action.Invoke(evt);
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void Log(string message)
        {
            if (_logger == null)
            {
                _logger = GameLogger.GetOrAdd<TChild>();
            }
            _logger.Info(message);
        }
    }
}
