using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager> {
    public Dictionary<string, Enemy> Units { get; private set; }

    public void SpawnUnit(string name, Vector3 location) {
        Enemy enemy = Units[name];
        if (enemy != null) {
            Instantiate(enemy, location, Quaternion.identity);
            //==============TODO============
            //change enemies/units to have base class and inheret down
            //change enemies stats to be loaded from scriptable object on creation like weapons

        }
    }


    public void DespawnUnit() {
        //==============TODO============
        //Sent unity to object pool 
        //Implement working object pool

        throw new NotImplementedException();
    }



}
