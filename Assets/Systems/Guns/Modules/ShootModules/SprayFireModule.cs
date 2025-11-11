using System.Collections.Generic;
using Systems.Guns.Modules.ShootModules;
using UnityEngine;

public class SprayFireModule : ShootModuleBase {
    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private Transform _bulletOrigin;

    [SerializeField]
    private byte _bulletsPerShoot = 8;

    [SerializeField]
    private float _spreadAngle = 10f;

    [SerializeField]
    private float _projectileSpeed = 50f;

    private PelletCollection _pelletCollection;

    protected override void Awake() {
        _pelletCollection = PelletCollection
            .For(_projectile)
            .WithPelletsPerShoot(_bulletsPerShoot)
            .WithSpreadAngle(_spreadAngle)
            .WithOrigin(_bulletOrigin);
        base.Awake();
    }

    public void Fire() {
        if (!fireRateController.IsReadyToFire) {
            return;
        }

        _pelletCollection
            .Create()
            .ForEach(p => {
                p.Init(p => Destroy(p.gameObject), _projectileSpeed).Fire();
            });

        fireRateController.Fired();
    }

    public override void Pressed() {
        Fire();
    }

    public override void Released() {
    }
}

public sealed class PelletCollection {
    private float _spreadAngle = 1f;
    private byte _pelletsPerShoot = 4;
    private Transform _origin;

    private readonly Projectile _projectile;

    private PelletCollection(Projectile projectile) {
        _projectile = projectile;
    }

    public static PelletCollection For(Projectile projectile) => new(projectile);

    public PelletCollection WithSpreadAngle(float angle) {
        _spreadAngle = angle;
        return this;
    }

    public PelletCollection WithPelletsPerShoot(byte count) {
        _pelletsPerShoot = count;
        return this;
    }

    public PelletCollection WithOrigin(Transform origin) {
        _origin = origin;
        return this;
    }

    public List<Projectile> Create() {
        var bullets = new List<Projectile>();
        for (int i = 0; i < _pelletsPerShoot - 1; i++) {
            float yaw = Random.Range(-_spreadAngle, _spreadAngle);
            float pitch = Random.Range(-_spreadAngle, _spreadAngle);

            Quaternion spreadRotation = Quaternion.Euler(pitch, yaw, 0);
            var rotation = _origin.rotation * spreadRotation;

            var bulletInstance = Object.Instantiate(_projectile, _origin.position, rotation);
            bullets.Add(bulletInstance);
        }
        return bullets;
    }
}


