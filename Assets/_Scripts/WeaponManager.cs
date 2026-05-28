using System.Collections.Generic;
using Systems.Guns;
using Systems.Guns.Interfaces;
using Systems.Guns.Utils;
using Systems.Inventories;
using Systems.Shared.Loggers;
using UnityEngine;

public interface IWeaponManager
{
    IWeapon CurrentWeapon { get; }
    void Equip(int index);
    void Equip(IWeapon weapon);
    void Pickup<TWeapon>(IConfig<TWeapon> config)
        where TWeapon : MonoBehaviour, IWeapon;
}

public sealed class WeaponManager : MonoBehaviour, IWeaponManager
{
    private const byte MaxInventorySize = 3;


    [SerializeField] private List<GameObject> _gWeapons = new(3);
    [SerializeField] private Inventory _scriptableObjectIntentory;
    [SerializeField] private Transform _transform;

    private readonly List<IWeapon> _weapons = new(3);
    private IGameLogger _logger;

    public IWeapon CurrentWeapon { get; private set; }

    private void Start()
    {
        _logger = GameLogger.GetOrAdd<WeaponManager>();
        //_weapons.AddRange(_scriptableObjectIntentory.GetItems());
    }

    public void Equip(IWeapon weapon) => EquipWeapon(_weapons.Find(w => w == weapon));

    public void Equip(int index)
    {
        var gameObject = _gWeapons[index];
        if (gameObject.TryGetComponent<Gun>(out var gun))
        {
            EquipWeapon(gun);
            return;
        }

        _logger.Error("weapon not found", this);
    }

    public void Pickup<TWeapon>(IConfig<TWeapon> config)
        where TWeapon : MonoBehaviour, IWeapon
    {
        if (IsInventoryFull)
        {
            return;
        }

        var weapon = WeaponFactory.Instance.Create(config);
        _weapons.Add(weapon);
    }

    private bool IsInventoryFull => _weapons.Count >= MaxInventorySize;

    private void EquipWeapon(IWeapon weapon)
    {
        if (weapon == null)
        {
            return;
        }

        SetActive(CurrentWeapon, false);
        SetActive(weapon, true);

        CurrentWeapon = weapon;
    }

    private void SetActive(IWeapon weapon, bool active)
    {
        if (weapon is MonoBehaviour mb)
        {
            mb.gameObject.SetActive(active);
            mb.gameObject.transform.SetParent(transform);
            mb.gameObject.transform.SetPositionAndRotation(
                transform.position,
                gameObject.transform.rotation
            );
        }
    }
}