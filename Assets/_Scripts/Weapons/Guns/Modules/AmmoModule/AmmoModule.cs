using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class AmmoModule : MonoBehaviour {
    [SerializeField] int _magazineSize;
    [SerializeField] int _currentAmmo;
    [Range(0f, 10f)]
    [SerializeField] float _reloadSpeed;

    public event Action OnReload;
    public event Action OnReloadEnd;
    public event Action<int> OnAmmoChange;

    bool _isReloading = false;

    virtual public int CurrentAmmo {
        get => _currentAmmo; protected set {
            if (_currentAmmo == value) {
                return;
            }
            _currentAmmo = value;
            OnAmmoChange(value);
        }
    }

    virtual public int MagazineSize {
        get => _magazineSize; protected set {
            _magazineSize = value;
        }
    }

    virtual public float ReloadSpeed {
        get => _reloadSpeed; protected set {
            _reloadSpeed = value;
        }
    }

    public virtual void Init(GunData data) {
        _currentAmmo = data.CurrentAmmo;
        _magazineSize = data.MagazineSize;
        _reloadSpeed = data.ReloadSpeed;
    }

    public abstract IEnumerator Reload();
    public abstract void DecreaseAmmo(int amount = 1);
    public abstract void IncreaseAmmo(int amount = 1);

    protected virtual void StartReload() {
        if (_isReloading) {
            return;
        }
        _isReloading = true;
        OnReload?.Invoke();
    }

    protected virtual void FinishReload() {
        _isReloading = false;
        CurrentAmmo = MagazineSize;
        OnReloadEnd?.Invoke();
    }
}
