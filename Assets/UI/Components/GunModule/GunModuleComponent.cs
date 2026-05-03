using System;
using System.Collections.Generic;
using Systems.Weapons.Guns.Modules;
using UnityEngine.UIElements;

namespace UI.Components.GunModule
{
    public sealed class GunModuleComponent : VisualElement
    {
        public IGunModule Selected { get; private set; }

        public event Action<IGunModule> OnModuleSelected;

        private readonly Button _mainButton;
        private readonly VisualElement _dropdownListContainer;
        private bool _isOpen = false;

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (_dropdownListContainer != null)
                {
                    _dropdownListContainer.style.display = value
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;
                }
            }
        }

        public GunModuleComponent(VisualTreeAsset template, string defaultLabel)
        {
            style.position = Position.Relative;

            template.CloneTree(this);

            _mainButton = this.Q<Button>("main-button");
            _dropdownListContainer = this.Q<VisualElement>("dropdown-container");

            if (_mainButton != null)
            {
                _mainButton.text = defaultLabel;
                _mainButton.clicked += ToggleDropdown;
            }

            if (_dropdownListContainer != null)
            {
                IsOpen = false;
            }
        }

        private void ToggleDropdown() => IsOpen = !IsOpen;

        public void SetModules(List<IGunModule> items, IGunModule selected = null)
        {
            _dropdownListContainer?.Clear();

            foreach (var item in items)
            {
                var itemButton = new Button(() => SelectModule(item))
                {
                    text = item.Name
                };
                itemButton.style.fontSize = 24;
                _dropdownListContainer.Add(itemButton);

                if (selected is not null && item == selected)
                {
                    Selected = selected;
                    _mainButton.text = Selected.Name;
                }
            }
        }

        private void SelectModule(IGunModule module)
        {
            _mainButton.text = module.Name;
            ToggleDropdown();

            OnModuleSelected?.Invoke(module);
        }
    }
}
