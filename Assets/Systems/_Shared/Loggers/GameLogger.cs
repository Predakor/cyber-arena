using System;
using UnityEngine;

namespace Systems.Shared.Loggers
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error
    }

    public static class GameLogger
    {
        public const string SettingsPath = "Logger/LogSettings";
        private static LogSettings _settings;

        public static void Configure(LogSettings settings)
        {
            if (_settings == null)
            {
                GetSettingsOrDefault();
                return;
            }

            _settings = settings;
        }

        public static LogHandler<TType> GetOrAdd<TType>()
        {
            if (_settings == null)
            {
                GetSettingsOrDefault();
            }

            var rule = _settings.GetRuleOrDefault<TType>();
            return new LogHandler<TType>(rule);
        }

        internal static void Log<TType>(LogRule rule, string message, string prefix, LogLevel level, UnityEngine.Object unityContext = null)
        {
            if (!rule.Enabled)
            {
                return;
            }

            if (!rule.EnabledLevels.HasFlag(ToFlag(level)))
            {
                return;
            }

            string preffixedMessage = LogBuffer.Combine(prefix, message);

            Action logAction = level switch
            {
                LogLevel.Warning => () => Debug.LogWarning(preffixedMessage, unityContext),
                LogLevel.Error => () => Debug.LogError(preffixedMessage, unityContext),
                _ => () => Debug.Log(preffixedMessage, unityContext),
            };

            logAction();
        }

        private static LogLevelFlags ToFlag(LogLevel level) => level switch
        {
            LogLevel.Debug => LogLevelFlags.Debug,
            LogLevel.Info => LogLevelFlags.Info,
            LogLevel.Warning => LogLevelFlags.Warning,
            LogLevel.Error => LogLevelFlags.Error,
            _ => LogLevelFlags.None,
        };

        private static LogSettings GetSettingsOrDefault()
        {
            if (_settings != null)
            {
                return _settings;
            }

            var settings = Resources.Load<LogSettings>(SettingsPath);
            return _settings = settings != null
                ? settings
                : throw new Exception($"LogSettings asset not found at path: {SettingsPath}");
        }
    }

    public sealed record LogHandler<TType>(LogRule Rule)
    {
        private readonly string prefix = $"[{typeof(TType).Name}]:";
        public void Debug(string message, UnityEngine.Object context = null) => GameLogger.Log<TType>(Rule, message, prefix, LogLevel.Debug, context);
        public void Info(string message, UnityEngine.Object context = null) => GameLogger.Log<TType>(Rule, message, prefix, LogLevel.Info, context);
        public void Warn(string message, UnityEngine.Object context = null) => GameLogger.Log<TType>(Rule, message, prefix, LogLevel.Warning, context);
        public void Error(string message, UnityEngine.Object context = null) => GameLogger.Log<TType>(Rule, message, prefix, LogLevel.Error, context);
    }

    public static class LogBuffer
    {
        // ThreadStatic ensures safety if you log from background threads
        [ThreadStatic]
        private static System.Text.StringBuilder _perThreadBuilder;

        public static string Combine(string prefix, string message)
        {
            _perThreadBuilder ??= new System.Text.StringBuilder(256);
            _perThreadBuilder.Clear(); // Reset pointer without dropping the internal array

            _perThreadBuilder.Append(prefix);
            _perThreadBuilder.Append(' '); // Single char append is faster than string " "
            _perThreadBuilder.Append(message);

            return _perThreadBuilder.ToString();
        }

    }
}