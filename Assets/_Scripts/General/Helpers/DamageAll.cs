using UnityEngine;

public enum DamageFalloff {
    None,
    Linear,
    Exponential
}

[RequireComponent(typeof(GetAllInRadius))]
public class DamageAll : MonoBehaviour {
    [SerializeField] GetAllInRadius enemyDetection;
    [SerializeField] int _damage = 10;
    [SerializeField] float _radius = 1f;
    [SerializeField] DamageFalloff _damageFallofType = DamageFalloff.None;

    public int Damage { get => _damage; set => _damage = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public DamageFalloff CurrentDamageFalloff { get => _damageFallofType; set => _damageFallofType = value; }

    int CalculateFallofDamage(int damage, Vector3 targetPosition) {
        if (CurrentDamageFalloff == DamageFalloff.None) {
            return damage;
        }

        float factor = 1f;
        float distance = Mathf.Abs((transform.position - targetPosition).magnitude);

        switch (_damageFallofType) {
            case DamageFalloff.Linear: {
                    factor = 1 - (distance / _radius);
                    factor = Mathf.Clamp01(factor);
                    break;
                }
            case DamageFalloff.Exponential: {
                    factor = Mathf.Exp(-distance / _radius);
                    break;
                }
        }
        damage = Mathf.Max(Mathf.FloorToInt(damage * factor), 1);
        return damage;
    }

    void Awake() {
        if (enemyDetection == null) {
            enemyDetection = GetComponent<GetAllInRadius>();
        }
    }
    public void InRadius() {
        Health[] objectsToDamage = enemyDetection.AllDamageable(transform.position, Radius);
        if (objectsToDamage.Length == 0) {
            return;
        }

        foreach (Health health in objectsToDamage) {
            health.Damage(CalculateFallofDamage(_damage, health.transform.position));
        }
    }

    public void InRadius(int damage) {
        Health[] objectsToDamage = enemyDetection.AllDamageable(transform.position, Radius);
        if (objectsToDamage.Length == 0) {
            return;
        }

        foreach (Health health in objectsToDamage) {
            health.Damage(CalculateFallofDamage(damage, health.transform.position));
        }
    }


    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
