using System;
using UnityEngine;

namespace Systems.Guns
{
    public sealed class DerivedSoSelectorAttribute : PropertyAttribute
    {
        public readonly Type BaseType;
        public DerivedSoSelectorAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }

    public sealed class TypedDerivedSOSelectorAttribute : PropertyAttribute { };


}
