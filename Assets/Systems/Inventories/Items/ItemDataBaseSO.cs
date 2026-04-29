using UnityEngine;

namespace Systems.Inventories.Items
{
    public interface IInvetoryItemBase
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        GameObject Model { get; }
        ScriptableObject Payload { get; }

    }

    [CreateAssetMenu(menuName = MenuPath)]
    public abstract class InventoryItemBase : ScriptableObject, IInvetoryItemBase
    {
        protected const string MenuPath = "Items/Data/";
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField, TextArea] public string Description { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public GameObject Model { get; protected set; }
        [field: SerializeField] public ScriptableObject Payload { get; protected set; }
    }
}
