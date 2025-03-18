using UnityEngine;

public class BossManager : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] Enemy _boss;

    public void Init(Enemy boss) {
        _boss = boss;
    }

    public void Init() {
        throw new System.NotImplementedException();
    }

}
