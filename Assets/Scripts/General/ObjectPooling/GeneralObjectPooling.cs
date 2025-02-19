using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralObjectPooling<T> : MonoBehaviour where T : MonoBehaviour {

    [SerializeField] protected Queue<T> _pool = new();

    [SerializeField] protected T prefab;
    [SerializeField] protected int initialSize = 10;
    [SerializeField] protected int maximumSize = 25;


    protected virtual void Awake() {
        for (int i = 0; i < initialSize; i++) {
            Return(Create());
        }
    }

    protected virtual T Create() {
        T obj = Instantiate(prefab, transform.parent);
        Debug.Log(obj);
        obj.gameObject.SetActive(false);
        return obj;
    }


    virtual public T Request() {
        if (_pool.Count > 0) {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return _pool.Count < maximumSize ? Create() : null;

    }

    virtual public void Return(T obj) {
        if (_pool.Count >= maximumSize) {
            Destroy(obj.gameObject);
            return;
        }

        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

}
