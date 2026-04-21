using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Channels.Weapons;
using Systems.Guns;
using Systems.Guns.Modules.AmmoModule.Base;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.ShootModules;
using Systems.Guns.Modules.SpreadModule;
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

        [Header("Module Lists")]
        [SerializeField] private List<SpreadModuleBase> _spreadModules;
        [SerializeField] private List<FireRateModuleBase> _fireRateModules;
        [SerializeField] private List<AmmoModuleBase> _ammoModules;
        [SerializeField] private List<ProjectileModuleBase> _projectileModules;


        [Header("UI References")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _rowStatTemplate;
        [SerializeField] private VisualTreeAsset _moduleTemplate;

        private VisualElement _statsContainer;
        private VisualElement _root;
        private VisualElement _modulesContainer;
        private bool _menuOpen = false;


        private Dictionary<Type, List<IGunModule>> _modulesMap;
        private Configuration _gunConfig;

        private void Awake()
        {
            gameObject.EnsureComponent(out _uiDocument);
            Debug.Assert(_weaponChannel != null, "Weapon Channel is missing", this);
            Debug.Assert(_inputsChannel != null, "Input channel is missing", this);

            _modulesMap = new Dictionary<Type, List<IGunModule>>()
            {
                { typeof(AmmoModuleBase), _ammoModules?.ConvertAll<IGunModule>(m => m) ?? new List<IGunModule>() },
                { typeof(SpreadModuleBase), _spreadModules?.ConvertAll<IGunModule>(m => m) ?? new List<IGunModule>() },
                { typeof(FireRateModuleBase), _fireRateModules?.ConvertAll<IGunModule>(m => m) ?? new List<IGunModule>() }
            };
        }
        private void Start()
        {
            _statsContainer = _uiDocument.rootVisualElement.Q<VisualElement>("stats-container");
            _modulesContainer = _uiDocument.rootVisualElement.Q<VisualElement>("modules-container");
            _root = _uiDocument.rootVisualElement.Q<VisualElement>("Container");
            _root.EnableInClassList("open", false);
        }

        private void OnEnable()
        {
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
            _gunConfig = new Configuration
            {
                fireRateModule = (FireRateModuleBase)e.Modules.FireRateModule,
                ammoModule = (AmmoModuleBase)e.Modules.AmmoModule,
                spreadModule = (SpreadModuleBase)e.Modules.SpreadModule,
                projectileModule = (ProjectileModuleBase)e.Modules.ProjectileModule,
            };
            AddModule(e.Modules.FireRateModule, typeof(FireRateModuleBase));
            AddModule(e.Modules.AmmoModule, typeof(AmmoModuleBase));
            AddModule(e.Modules.SpreadModule, typeof(SpreadModuleBase));
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

        private void AddModule(IGunModule module, Type moduleType)
        {
            if (module == null)
            {
                return;
            }

            var moduleDropdown = new GunModuleComponent(_moduleTemplate, module.Name);


            if (_modulesMap.TryGetValue(moduleType, out var availableModules))
            {
                moduleDropdown.SetModules(availableModules);
            }

            moduleDropdown.OnModuleSelected += HandleModuleSelection;

            _modulesContainer.Add(moduleDropdown);
        }

        private void HandleModuleSelection(IGunModule selectedModule)
        {
            switch (selectedModule)
            {
                case FireRateModuleBase fireRate:
                    _gunConfig.fireRateModule = fireRate;
                    break;
                case AmmoModuleBase ammo:
                    _gunConfig.ammoModule = ammo;
                    break;
                case SpreadModuleBase spread:
                    _gunConfig.spreadModule = spread;
                    break;
                case ProjectileModuleBase projectile:
                    _gunConfig.projectileModule = projectile;
                    break;
            }

            _weaponChannel.RaiseReconfigured(_gunConfig);

        }
    }


}
