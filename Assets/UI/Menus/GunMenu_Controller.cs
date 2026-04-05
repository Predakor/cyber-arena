using Assets.Scripts.Utils;
using Systems.Channels.Weapons;
using Systems.Guns;
using UI.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menus
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class GunMenu_Controller : MonoBehaviour
    {
        [SerializeField] private WeaponChannel _weaponChannel;
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _rowStatTemplate;

        private VisualElement _statsContainer;

        private void Awake()
        {
            gameObject.EnsureComponent(out _uiDocument);
        }

        private void OnEnable()
        {
            _statsContainer = _uiDocument.rootVisualElement.Q<VisualElement>("stats-container");
            _weaponChannel.Subscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
        }

        private void OnDisable()
        {
            _weaponChannel.Unsubscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
        }

        private void StatsChangeHandler(WeaponEvents.StatsChanged e)
        {
            _statsContainer.Clear();

            AddRow("Damage", e.Stats.Damage);
            AddRow("Speed", e.Stats.Speed);
            AddRow("Duration", e.Stats.Duration);
            AddRow("Size", e.Stats.Size);
            AddRow("Ammo Cost", e.Stats.AmmoCost);
            AddRow("Piercing", e.Stats.Piercing);
            AddRow("Crit Chance", e.Stats.CritChance);
            AddRow("Crit Damage", e.Stats.CritDamage);
            AddRow("Effect Radius", e.Stats.EffectRadius);
            AddRow("Effect Strength", e.Stats.EffectStrength);
            AddRow("Effect Duration", e.Stats.EffectDuration);

            foreach (var customStat in e.Stats.Custom)
            {
                AddRow(customStat.Name, customStat.Value);
            }
        }

        private void AddRow(string label, float value)
        {
            _statsContainer.Add(new StatRowElement(_rowStatTemplate, label, $"{value:F1}"));
        }

    }
}
