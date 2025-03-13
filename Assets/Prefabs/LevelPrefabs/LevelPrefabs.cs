using Helpers.Collections;
using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Floor Generation/Floor prefabs", fileName = "Level_Prefabs")]
public class LevelPrefabs : ScriptableObject {
    public LevelPrefab[] floorPrefabs;
    public LevelPrefab[] wallPrefabs;
    public LevelPrefab[] doorPrefabs;

    T GetRandom<T>(T[] collection) => CollectionHelpers.RandomElement(collection);
    public LevelPrefab RandomFloor() => GetRandom(floorPrefabs);
    public LevelPrefab RandomWall() => GetRandom(wallPrefabs);
    public LevelPrefab RandomDoor() => GetRandom(doorPrefabs);

    public GameObject RandomFloorPrefab() => GetRandom(floorPrefabs).prefab;
    public GameObject RandomWallPrefab() => GetRandom(wallPrefabs).prefab;
    public GameObject RandomDoorPrefab() => GetRandom(doorPrefabs).prefab;

    [ContextMenu("auto complete dimmensions")]
    void ValidateDimensions() {
        foreach (var prefab in floorPrefabs) { prefab.Validate(); }
        foreach (var prefab in wallPrefabs) { prefab.Validate(); }
        foreach (var prefab in doorPrefabs) { prefab.Validate(); }
    }
}

[Serializable]
public struct LevelPrefab {
    public GameObject prefab;
    public Vector3 dimensions;
    public Vector3 rotations;

    public void Validate() {
        if (prefab.TryGetComponent<Renderer>(out var renderer)) {
            Debug.Log(dimensions);
            dimensions = renderer.bounds.size;
            return;
        }

        Renderer childRenderer = prefab.GetComponentInChildren<Renderer>();
        if (childRenderer) {
            dimensions = childRenderer.bounds.size;
            Debug.Log(dimensions);
            return;
        }

        Debug.LogWarning("No rendered found assign dimmension by hand", prefab);
    }

}