using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CorridorGenerator : MonoBehaviour {

    [Header("Templates")]
    [SerializeField] FloorPrefabs _prefabs;

    [Header("Variables")]
    [SerializeField] Vector3 _size; //x = width; y=height;z=length
    [SerializeField] Vector2 _segments; //x width segments y length segments
    [SerializeField] Vector3 _direction;

    BoxCollider _collider;

    public void LoadPrefabs(FloorPrefabs florPrefabs) => _prefabs = florPrefabs;

    public void LoadData(CorridorData data) {
        _size = data.size;
        _segments = data.segments;
        _direction = data.direction;
        _prefabs = data.prefabs;

        HandleCollider();
    }

    public void GenerateCorridor() {
        GameObject randomWall = _prefabs.RandomWall();
        GameObject randomFloor = _prefabs.RandomFloor();

        Vector3 position = transform.position;

        InstantiateCorridorFloor(randomFloor, position);
        InstantiateCorridorWalls(randomWall, position);

        float angle = Vector3.Angle(_direction, Vector3.forward);
        transform.Rotate(new(0, 1, 0), angle);
    }


    void Awake() {
        HandleCollider();
    }

    void InstantiateCorridorFloor(GameObject floor, Vector3 position) {
        Vector3 scale = CalculateScale();
        GameObject floorSegment = CreateObject(floor, position, scale);

    }
    void InstantiateCorridorWalls(GameObject walll, Vector3 position) {
        float width = _size.x;
        float wall_Width = 0f;

        Vector3 scale = CalculateScale();
        Vector3 perpendicularVector = new(1, 0, 0);

        float distanceToEdge = (width / 2) + (wall_Width / 2);
        Vector3 wallOffset = perpendicularVector * distanceToEdge;
        Vector3 rotationOffset = new(0, 180, 0);

        for (int i = 0; i < 2; i++) {
            GameObject gameObject = CreateObject(walll, position + wallOffset, scale);
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

    //placeholder remove when floor prefabs will have sizes
    Vector3 CalculateScale() {
        const int base_width = 10;
        const int base_lenght = 10;
        float widthScale = _size.x / base_width / _segments.x;
        float lenghtScale = _size.z / base_lenght / _segments.y;
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
    public FloorPrefabs prefabs;
    public Vector3 direction;
    public Vector2 segments;
    public Vector3 size;
}

