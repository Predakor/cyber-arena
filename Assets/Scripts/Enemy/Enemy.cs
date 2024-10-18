using UnityEngine;

[RequireComponent(typeof(GeneralAi))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {

    [SerializeField] GeneralAi AI;
    [SerializeField] Movement Controller;
    [SerializeField] Health Health;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
