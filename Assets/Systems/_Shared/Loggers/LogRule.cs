using System;
using UnityEngine;

namespace Systems.Shared.Loggers
{
    [Flags]
    public enum LogLevelFlags
    {
        None = 0,
        Debug = 1 << 0,
        Info = 1 << 1,
        Warning = 1 << 2,
        Error = 1 << 3,
        All = Debug | Info | Warning | Error
    }

    public enum LogGroup
    {
        Other = 0,
        Channels,
        Guns,
        Rooms,
        Managers
    }

    [Serializable]
    public sealed class LogRule
    {
        [SerializeField] public string ClassName;
        [SerializeField] public bool Enabled;
        [SerializeField] public LogLevelFlags EnabledLevels;
        [SerializeField] public LogGroup Group = LogGroup.Other;
    }
}