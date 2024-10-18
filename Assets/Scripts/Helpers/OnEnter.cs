using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnEnter : MonoBehaviour {

    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            onEnter.Invoke();
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            onExit.Invoke();
        }
    }
}
