using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour {


    [SerializeField] Collider interactionColider;

    public UnityEvent onEnterEvenet;
    public UnityEvent onExitEvent;


    //colider to show player item can be picked
    //
    // Start is called before the first frame update




    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Player")) {
            onEnterEvenet.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            onExitEvent.Invoke();
        }
    }

    void Start() {

    }


    void onPickup() {

    }
    void onDrop() {

    }

    void onEquip() { }

    // Update is called once per frame
    void Update() {

    }
}
