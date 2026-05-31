using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace UI.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UIElementAttribute : Attribute
    {
        public string Name { get; }

        public UIElementAttribute(string name)
        {
            Name = name;
        }
    }
    public static class UIBinder
    {
        private static readonly Dictionary<Type, FieldInfo[]> _cache = new();

        public static void Bind(object target, VisualElement provider)
        {
            Type type = target.GetType();
            if (!_cache.TryGetValue(type, out var fields))
            {
                fields = type
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute<UIElementAttribute>() != null)
                    .ToArray();

                _cache[type] = fields;
            }
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<UIElementAttribute>();

                var element = provider.Q(attr.Name)
                    ?? throw new Exception($"[UIBinder] Could not find VisualElement '{attr.Name}' on {target.GetType().Name}");

                // Inject the found element into the field
                field.SetValue(target, element);
            }
        }

        public static void Unbind(object target)
        {
            Type type = target.GetType();
            if (_cache.TryGetValue(type, out var fields))
            {
                foreach (var field in fields)
                {
                    field.SetValue(target, null);
                }
            }
        }
    }
}
