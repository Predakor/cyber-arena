using UnityEngine;
using UnityEngine.Events;

public class InvokeEvent : MonoBehaviour {
    public UnityEvent OnAwake;
    public UnityEvent OnStart;
    public UnityEvent OnEnabled;
    public UnityEvent OnDisabled;
    public UnityEvent OnDestroyed;

    void Awake() {
        OnAwake?.Invoke();
    }
    void Start() {
        OnStart?.Invoke();
    }
    void OnEnable() {
        OnEnabled?.Invoke();
    }
    void OnDisable() {
        OnDisabled?.Invoke();
    }
    void OnDestroy() {
        OnDestroyed?.Invoke();
    }
}
