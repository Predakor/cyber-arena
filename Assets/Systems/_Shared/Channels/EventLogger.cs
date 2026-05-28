using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Systems.Shared.Loggers;

namespace Systems.Shared.Channels
{
    /// <summary>Enables associating event containers with their respective channels for logging purposes.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ChannelEventsAttribute : Attribute
    {
        public Type ChannelType { get; }

        public ChannelEventsAttribute(Type channelType)
        {
            ChannelType = channelType;
        }
    }

    [Serializable]
    public class EventLogRule
    {
        public string EventName;
        public bool Enabled;
    }

    [Serializable]
    public sealed class EventChannellLoger<TChannel>
        where TChannel : EventChannelBase<TChannel>
    {
        private readonly UnityEngine.Object owner;
        private IGameLogger _logger;

        private readonly List<EventLogRule> _eventLogRules;
        private Dictionary<string, EventLogRule> _eventLogMap;

        public EventChannellLoger(UnityEngine.Object owner, List<EventLogRule> eventLogRules)
        {
            this.owner = owner;
            _eventLogRules = eventLogRules ?? new();
            EnsureEventLogMap();
        }

        public void LogEvent(Type eventType, string message)
        {
            if (!IsEventLoggingEnabled(eventType))
            {
                return;
            }

            _logger ??= GameLogger.GetOrAdd<TChannel>();
            _logger.Info(message);
        }

        private bool IsEventLoggingEnabled(Type eventType)
        {
            EnsureEventLogMap();

            Type container = eventType.DeclaringType;
            if (container == null)
            {
                return true;
            }

            if (_eventLogMap.TryGetValue(container.Name, out var rule))
            {
                return rule.Enabled;
            }

            return true;
        }

        public void EnsureEventLogMap()
        {
            _eventLogMap ??= _eventLogRules
                .Where(x => !string.IsNullOrWhiteSpace(x.EventName))
                .ToDictionary(
                    x => x.EventName,
                    x => x
                );
        }

        public void SyncEventLogRules()
        {
            var stopper = new Stopwatch();
            var eventTypes = GetAllEventTypes().ToList();
            if (eventTypes.Count == 0)
            {
                return;
            }

            var existing = new HashSet<string>(
                _eventLogRules.Select(r => r.EventName).Where(n => !string.IsNullOrWhiteSpace(n)),
                StringComparer.Ordinal
            );

            foreach (var container in eventTypes)
            {
                if (existing.Contains(container.Name))
                {
                    continue;
                }

                _eventLogRules.Add(new EventLogRule
                {
                    EventName = container.Name,
                    Enabled = true
                });
            }

            UnityEditor.EditorUtility.SetDirty(owner);
        }

        private static IEnumerable<Type> GetAllEventTypes()
        {
            Assembly channelsAssembly = typeof(EventChannellLoger<>).Assembly;
            string channelsTargetName = channelsAssembly.GetName().Name;

            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> targetAssemblies = new();

            for (int i = 0; i < allAssemblies.Length; i++)
            {
                Assembly assembly = allAssemblies[i];
                if (assembly.IsDynamic)
                {
                    continue;
                }

                if (assembly == channelsAssembly)
                {
                    targetAssemblies.Add(assembly);
                    continue;
                }

                AssemblyName[] referencedNames = assembly.GetReferencedAssemblies();
                bool isDependencyMatched = false;

                for (int j = 0; j < referencedNames.Length; j++)
                {
                    if (string.Equals(referencedNames[j].Name, channelsTargetName, StringComparison.Ordinal))
                    {
                        isDependencyMatched = true;
                        break;
                    }
                }

                if (isDependencyMatched)
                {
                    targetAssemblies.Add(assembly);
                }
            }

            Type[] types;
            for (int i = 0; i < targetAssemblies.Count; i++)
            {
                try
                {
                    types = targetAssemblies[i].GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t is not null).ToArray();
                }

                if (types == null)
                {
                    continue;
                }

                for (int j = 0; j < types.Length; j++)
                {
                    Type type = types[j];

                    if (type == null || !type.IsClass)
                    {
                        continue;
                    }

                    if (type.DeclaringType is not null)
                    {
                        continue;
                    }

                    if (!(type.IsAbstract && type.IsSealed))
                    {
                        continue;
                    }

                    if (!IsContainerForChannel(type))
                    {
                        continue;
                    }

                    Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
                    if (nestedTypes is null)
                    {
                        continue;
                    }

                    for (int k = 0; k < nestedTypes.Length; k++)
                    {
                        Type nested = nestedTypes[k];
                        if (nested is not null)
                        {
                            yield return nested;
                        }
                    }
                }
            }
        }

        private static bool IsContainerForChannel(Type container)
        {
            var attribute = (ChannelEventsAttribute)Attribute.GetCustomAttribute(
                container,
                typeof(ChannelEventsAttribute),
                false
            );

            if (attribute == null)
            {
                return false;
            }

            return attribute.ChannelType == typeof(TChannel);
        }
    }
}
