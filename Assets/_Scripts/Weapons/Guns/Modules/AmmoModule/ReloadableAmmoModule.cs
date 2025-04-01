using System.Collections;
using UnityEngine;

public class ReloadableAmmoModule : AmmoModule {
    public override IEnumerator Reload() {
        if (CurrentAmmo < MagazineSize) {
            yield break;
        }
        StartReload();
        yield return new WaitForSeconds(ReloadSpeed);
        FinishReload();
    }

    public override void DecreaseAmmo(int amount = 1) {
        CurrentAmmo -= amount;
    }

    public override void IncreaseAmmo(int amount = 1) {
        CurrentAmmo += amount;
    }
}
