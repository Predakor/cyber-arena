using Assets.Scripts.Utils;
using Systems.Channels;
using Systems.Channels.Inputs;
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
        [SerializeField] private InputsChannel _inputsChannel;
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _rowStatTemplate;

        private VisualElement _statsContainer;
        private VisualElement _root;
        private bool _menuOpen = false;

        private void Awake()
        {
            gameObject.EnsureComponent(out _uiDocument);
            Debug.Assert(_weaponChannel != null, "Weapon Channel is missing", this);
            Debug.Assert(_inputsChannel != null, "Input channel is missing", this);
        }

        private void OnEnable()
        {
            _statsContainer = _uiDocument.rootVisualElement.Q<VisualElement>("stats-container");
            _root = _uiDocument.rootVisualElement.Q<VisualElement>("Container");

            _root.EnableInClassList("open", false);

            _weaponChannel.Subscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
            _inputsChannel.Subscribe<InputEvents.ConfigureWeapon>(ConfigureWeaponHandler);
        }

        private void OnDisable()
        {
            _weaponChannel.Unsubscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
            _inputsChannel.Unsubscribe<InputEvents.ConfigureWeapon>(ConfigureWeaponHandler);
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

        private void ConfigureWeaponHandler(InputEvents.ConfigureWeapon e)
        {
            _menuOpen = !_menuOpen;
            _root.EnableInClassList("open", _menuOpen);
        }

        private void AddRow(string label, float value)
        {
            _statsContainer.Add(new StatRowElement(_rowStatTemplate, label, $"{value:F1}"));
        }

    }
}
