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

            Debug.Assert(_nameLabel != null, "Missing stat-label in RowStat template");
            Debug.Assert(_valueLabel != null, "Missing stat-value in RowStat template");

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
