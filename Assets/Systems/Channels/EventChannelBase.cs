using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Channels
{
    public interface IEventChannel
    {
        void Raise<TEvent>(TEvent evt);
        void Subscribe<TEvent>(Action<TEvent> handler);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
    }

    public abstract class EventChannelBase : ScriptableObject, IEventChannel
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        protected const string MenuName = "Channels/";

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
        }

        public void Raise<TEvent>(TEvent evt)
        {
            if (_handlers.TryGetValue(typeof(TEvent), out var del) && del is Action<TEvent> action)
            {
                action.Invoke(evt);
            }
        }
    }
}
