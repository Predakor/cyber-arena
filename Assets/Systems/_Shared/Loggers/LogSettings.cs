using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Shared.Loggers
{
    [CreateAssetMenu(menuName = "Settings/Log Settings", fileName = "LogSettings")]
    public sealed class LogSettings : ScriptableObject
    {
        [SerializeField] private LogLevelFlags _defaultEnabledLevels = LogLevelFlags.Info | LogLevelFlags.Warning | LogLevelFlags.Error;
        [SerializeField] private List<LogRule> _rules = new();

        private Dictionary<string, LogRule> _ruleMap;

        [ContextMenu("Refresh Rules")]
        public void RefreshRules()
        {
            _ruleMap = _rules
                .Where(r => !string.IsNullOrWhiteSpace(r.ClassName))
                .ToDictionary(x => x.ClassName, x => x, StringComparer.Ordinal);
        }

        [ContextMenu("Enable All Rules")]
        public void EnableAll() => SetAllRulesState(true);

        [ContextMenu("Disable All Rules")]
        public void DisableAll() => SetAllRulesState(false);

        public LogRule GetRuleOrDefault<TType>(LogGroup group)
        {
            if (_ruleMap == null)
            {
                RefreshRules();
            }

            var ruleName = typeof(TType).Name;
            if (_ruleMap.TryGetValue(ruleName, out var rule))
            {
                return rule;
            }

            rule = new LogRule
            {
                ClassName = typeof(TType).Name,
                Enabled = true,
                EnabledLevels = _defaultEnabledLevels,
                Group = group,
            };

            _rules.Add(rule);
            _ruleMap[ruleName] = rule;

            return rule;

        }


        private void SetAllRulesState(bool enabled)
        {
            foreach (var rule in _rules)
            {
                rule.Enabled = enabled;
            }
        }
    }

}