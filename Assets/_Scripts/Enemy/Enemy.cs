using UnityEngine;

[RequireComponent(typeof(GeneralHostileAi))]
[RequireComponent(typeof(BaseMovement))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {

    public GeneralHostileAi AI;
    public BaseMovement Controller;
    public Health Health;


    public void Freeze() {
        AI.enabled = false;
        Controller.enabled = false;
        Health.enabled = false;
    }

    public void ActivateEnemy() {
        AI.enabled = true;
        Controller.enabled = true;
        Health.enabled = true;
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
