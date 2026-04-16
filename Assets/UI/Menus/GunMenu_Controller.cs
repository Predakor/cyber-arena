using Assets.Scripts.Utils;
using System.Collections.Generic;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Channels.Weapons;
using Systems.Guns;
using Systems.Weapons.Guns.Modules;
using UI.Components;
using UI.Components.GunModule;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menus
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class GunMenu_Controller : MonoBehaviour
    {
        [SerializeField] private WeaponChannel _weaponChannel;
        [SerializeField] private InputsChannel _inputsChannel;

        [Header("UI References")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _rowStatTemplate;
        [SerializeField] private VisualTreeAsset _moduleTemplate;

        private VisualElement _statsContainer;
        private VisualElement _root;
        private VisualElement _modulesContainer;
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
            _modulesContainer = _uiDocument.rootVisualElement.Q<VisualElement>("modules-container");
            _root = _uiDocument.rootVisualElement.Q<VisualElement>("Container");

            _root.EnableInClassList("open", false);

            _weaponChannel.Subscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
            _weaponChannel.Subscribe<WeaponEvents.ModulesChanged>(ModulsChangedHandler);
            _inputsChannel.Subscribe<InputEvents.ConfigureWeapon>(ConfigureWeaponHandler);
        }

        private void OnDisable()
        {
            _weaponChannel.Unsubscribe<WeaponEvents.StatsChanged>(StatsChangeHandler);
            _weaponChannel.Unsubscribe<WeaponEvents.ModulesChanged>(ModulsChangedHandler);

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

        private void ModulsChangedHandler(WeaponEvents.ModulesChanged e)
        {
            _modulesContainer.Clear();
            AddModule(e.Modules.FireRateModule);
            AddModule(e.Modules.AmmoModule);
            AddModule(e.Modules.SpreadModule);
        }

        private void ConfigureWeaponHandler(InputEvents.ConfigureWeapon e)
        {
            Debug.Log(e.ToString());
            _menuOpen = !_menuOpen;
            _root.EnableInClassList("open", _menuOpen);
        }

        private void AddRow(string label, float value)
        {
            _statsContainer.Add(new StatRowElement(_rowStatTemplate, label, $"{value:F1}"));
        }

        private void AddModule(IGunModule module)
        {
            var moduleDropdown = new GunModuleComponent(_moduleTemplate, module.Name);

            var choices = new List<string> { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" };
            moduleDropdown.SetItems(choices);

            moduleDropdown.OnItemSelected += (selectedItem) =>
            {
                Debug.Log($"Module changed to {selectedItem}");
            };

            _modulesContainer.Add(moduleDropdown);
        }
    }


}
