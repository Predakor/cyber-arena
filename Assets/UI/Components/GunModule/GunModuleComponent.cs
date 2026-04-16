using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UI.Components.GunModule
{
    public sealed class GunModuleComponent : VisualElement
    {
        public event Action<string> OnItemSelected;

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


        //should accept IGunModule and show its name and possible options, for now just a list of strings
        public void SetItems(List<string> items)
        {
            _dropdownListContainer?.Clear();

            foreach (var item in items)
            {
                var itemButton = new Button(() => SelectItem(item)) { text = item };
                _dropdownListContainer.Add(itemButton);
            }
        }

        private void SelectItem(string item)
        {
            _mainButton.text = item;
            ToggleDropdown();

            OnItemSelected?.Invoke(item);
        }
    }
}
