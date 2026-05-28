using Helpers.Collections;
using System;
using Systems.Shared.Loggers;
using UnityEngine;


[CreateAssetMenu(menuName = "Floor Generation/Floor prefabs", fileName = "Level_Prefabs")]
public sealed class LevelPrefabs : ScriptableObject
{
    public LevelPrefab[] floorPrefabs;
    public LevelPrefab[] wallPrefabs;
    public LevelPrefab[] doorPrefabs;

    private T GetRandom<T>(T[] collection) => CollectionUtils.RandomElement(collection);
    public LevelPrefab RandomFloor() => GetRandom(floorPrefabs);
    public LevelPrefab RandomWall() => GetRandom(wallPrefabs);
    public LevelPrefab RandomDoor() => GetRandom(doorPrefabs);

    public GameObject RandomFloorPrefab() => GetRandom(floorPrefabs).prefab;
    public GameObject RandomWallPrefab() => GetRandom(wallPrefabs).prefab;
    public GameObject RandomDoorPrefab() => GetRandom(doorPrefabs).prefab;

    [ContextMenu("auto complete dimmensions")]
    private void ValidateDimensions()
    {
        var logger = GameLogger.GetOrAdd<LevelPrefabs>();
        foreach (var prefab in floorPrefabs)
        {
            if (prefab.TryAutoAssingnDimennsion(out var message))
            {
                logger.Error(message, prefab.prefab);
            }
        }
        foreach (var prefab in wallPrefabs)
        {
            if (prefab.TryAutoAssingnDimennsion(out var message))
            {
                logger.Error(message, prefab.prefab);
            }
        }
        foreach (var prefab in doorPrefabs)
        {
            if (prefab.TryAutoAssingnDimennsion(out var message))
            {
                logger.Error(message, prefab.prefab);
            }
        }
    }
}

[Serializable]
public struct LevelPrefab
{
    public GameObject prefab;
    public Vector3 dimensions;
    public Vector3 rotations;

    public bool TryAutoAssingnDimennsion(out string message)
    {
        message = string.Empty;
        if (prefab.TryGetComponent<Renderer>(out var renderer))
        {
            dimensions = renderer.bounds.size;
            return true;
        }

        Renderer childRenderer = prefab.GetComponentInChildren<Renderer>();
        if (childRenderer)
        {
            dimensions = childRenderer.bounds.size;
            return true;
        }

        message = "No rendered found assign dimmension by hand";
        return false;
    }

}