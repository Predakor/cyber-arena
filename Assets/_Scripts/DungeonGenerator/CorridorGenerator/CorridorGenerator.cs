using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CorridorGenerator : MonoBehaviour {

    [Header("Templates")]
    [SerializeField] LevelPrefabs _prefabs;

    [Header("Variables")]
    [SerializeField] Vector3 _size; //x = width; y=height;z=length
    [SerializeField] Vector2 _segments; //x width segments y length segments
    [SerializeField] Vector3 _direction;

    BoxCollider _collider;

    public void LoadPrefabs(LevelPrefabs florPrefabs) => _prefabs = florPrefabs;

    public void LoadData(CorridorData data) {
        _size = data.size;
        _segments = data.segments;
        _direction = data.direction;
        _prefabs = data.prefabs;

        HandleCollider();
    }

    public void GenerateCorridor() {
        LevelPrefab randomWall = _prefabs.RandomWall();
        LevelPrefab randomFloor = _prefabs.RandomFloor();

        Vector3 position = transform.position;

        InstantiateCorridorFloor(randomFloor, position);
        InstantiateCorridorWalls(randomWall, position);

        RotateTowardsNextRoom();
    }

    void Awake() {
        HandleCollider();
    }

    void RotateTowardsNextRoom() {
        float angle = RoomHelpers.DirectionToAngle(_direction);
        Vector3 rotationAxis = new(0, 1, 0);
        transform.Rotate(rotationAxis, angle);
    }

    void InstantiateCorridorFloor(LevelPrefab floor, Vector3 position) {
        Vector3 scale = CalculateScale(floor.dimensions, _size);
        CreateObject(floor.prefab, position, scale);
    }

    void InstantiateCorridorWalls(LevelPrefab walll, Vector3 position) {
        Vector3 scale = CalculateScale(walll.dimensions, _size);
        Vector3 perpendicularVector = new(1, 0, 0);

        float distanceToEdge = (_size.x / 2) + (walll.dimensions.y / 2);
        Vector3 wallOffset = perpendicularVector * distanceToEdge;
        Vector3 rotationOffset = new(0, 180, 0);

        for (int i = 0; i < 2; i++) {
            GameObject gameObject = CreateObject(walll.prefab, position + wallOffset, scale);
            gameObject.transform.Rotate(rotationOffset);
            rotationOffset.y = 0;
            wallOffset *= -1;
        }
    }

    Vector3 CalculateScale(Vector3 baseSize, Vector3 targetSize) {
        float widthScale = targetSize.x / baseSize.x / _segments.x;
        float lenghtScale = targetSize.z / baseSize.z / _segments.y;
        return new(widthScale, 1, lenghtScale);
    }

    GameObject CreateObject(GameObject obj, Vector3 pos, Vector3 scale) {
        GameObject generatedObject = Instantiate(obj, pos, Quaternion.identity, transform);
        generatedObject.transform.localScale = scale;
        return generatedObject;
    }

    void HandleCollider() {
        if (!_collider) {
            _collider = GetComponent<BoxCollider>();
        }
        _collider.size = _size;
    }

}
public struct CorridorData {
    public LevelPrefabs prefabs;
    public Vector3 direction;
    public Vector2 segments;
    public Vector3 size;
}

