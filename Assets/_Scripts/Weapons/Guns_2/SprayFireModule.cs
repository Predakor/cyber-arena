using System.Collections.Generic;
using UnityEngine;

public class SprayFireModule : ShootModule, IFireModule {
    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private Transform _bulletOrigin;

    [SerializeField]
    private byte _bulletsPerShoot = 8;

    [SerializeField]
    private float _fireRate = 1.0f;

    [SerializeField]
    private float _spreadAngle = 10f;

    [SerializeField]
    private float _projectileSpeed = 50f;

    private FireRateController _fireRateController;
    private PelletCollection _pelletCollection;

    private void Awake() {
        _fireRateController = FireRateController.FromRoundsPerSecond(_fireRate);
        _pelletCollection = PelletCollection
            .For(_projectile)
            .WithPelletsPerShoot(_bulletsPerShoot)
            .WithSpreadAngle(_spreadAngle)
            .WithOrigin(_bulletOrigin);
    }

    [ContextMenu("Fire the fucker")]
    public void Fire() {
        if (_fireRateController.IsReadyToFire) {
            _pelletCollection
                .Create()
                .ForEach(p => {
                    p.Init(null, _projectileSpeed)
                    .Fire();
                });

            _fireRateController.Fired();
        }
    }

    public override void Shoot(Projectile projectile) {
        Fire();
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

public sealed class FireRateController {
    private float _fireDelay;
    private float _lastFiretime = Time.time;

    private FireRateController(float fireDelay) {
        _fireDelay = fireDelay;
    }

    public static FireRateController FromRPM(float roundsPerMinute) =>
        new(1f / (roundsPerMinute / 60));

    public static FireRateController FromRoundsPerSecond(float roundsPerSeconds) =>
        new(roundsPerSeconds);

    public void Fired() => _lastFiretime = Time.time;

    public bool IsReadyToFire => Time.time >= _lastFiretime + _fireDelay;

    public FireRateController UpdateRPM(float roundsPerMinute) {
        _fireDelay = 1f / (roundsPerMinute / 60);
        return this;
    }
}
