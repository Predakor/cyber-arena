using UnityEngine;

public class RoomFactory : MonoBehaviour {
    [SerializeField] TemplatesHolderData _roomTemplates;
    [SerializeField] LevelPrefabs _levelPrefabs;
    [SerializeField] GameObject _roomPlaceholder;

    //templates
    //level prefabs
    // Start is called before the first frame update


    public void Init(TemplatesHolderData templates, LevelPrefabs prefabs) {
        _levelPrefabs = prefabs;
        _roomTemplates = templates;
    }

    public void PlaceNode(RoomNode node, Vector3 position) {

    }

    public void Construct(RoomNode node, Vector3 position) {
        //generate room on position

    }
}
