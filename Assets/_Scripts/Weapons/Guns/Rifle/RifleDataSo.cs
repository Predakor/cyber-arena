using UnityEngine;

[CreateAssetMenu(fileName = "Rifle_Data", menuName = "Item/Weapon/Rifle")]
public class RifleDataSo : GunData {
    [SerializeField] Vector3 _spread;
    public Vector3 Spread { get => _spread; }
}
