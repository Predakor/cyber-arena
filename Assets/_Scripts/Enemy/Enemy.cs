using UnityEngine;

[RequireComponent(typeof(GeneralHostileAi))]
[RequireComponent(typeof(BaseMovement))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {

    [SerializeField] GeneralHostileAi AI;
    [SerializeField] BaseMovement Controller;
    [SerializeField] Health Health;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
