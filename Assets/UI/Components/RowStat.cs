using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Components
{
    public sealed class StatRowElement : VisualElement
    {
        private readonly Label _nameLabel;
        private readonly Label _valueLabel;

        public StatRowElement(VisualTreeAsset template, string statName, string value)
        {
            template.CloneTree(this);

            _nameLabel = this.Q<Label>("stat-label");
            _valueLabel = this.Q<Label>("stat-value");

            if (_nameLabel is null)
            {
                Debug.LogError("Missing stat-label in RowStat template");
                return;
            }

            if (_valueLabel is null)
            {
                Debug.LogError("Missing stat-value in RowStat template");
                return;
            }

            _nameLabel.text = statName;
            _valueLabel.text = value;
        }

        public void SetValue(string value)
        {
            if (_valueLabel != null)
            {
                _valueLabel.text = value;
            }
        }

        public void SetLabel(string label)
        {
            if (_nameLabel != null)
            {
                _nameLabel.text = label;
            }
        }
    }
}
