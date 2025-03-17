using UnityEngine;

[CreateAssetMenu(fileName = "Level_Template_Holder", menuName = "Level Generation/Data/Templates")]
public class TemplatesHolderData : ScriptableObject {
    [Header("Room templates")]
    [SerializeField] GameObject roomTemplatePrefab;
    [SerializeField] RoomData roomDataTemplate;
    [SerializeField] RoomRestrictionsSO roomRestrictions;

    [Header("Corridor templates")]
    [SerializeField] GameObject corridorTemplatePrefab;

    public GameObject RoomTemplatePrefab => roomTemplatePrefab;
    public RoomData RoomDataTemplate => roomDataTemplate;
    public RoomRestrictionsSO RoomRestrictions => roomRestrictions;
    public GameObject CorridorTemplatePrefab => corridorTemplatePrefab;
}
