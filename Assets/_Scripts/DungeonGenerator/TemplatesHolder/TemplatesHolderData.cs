using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Template_Holder", menuName = "Level Generation/Data/Templates")]
public class TemplatesHolderData : ScriptableObject {
    [Header("Room templates")]
    [SerializeField] List<RoomTemplateEtry> _roomTemplatesList = new(6);
    [SerializeField] RoomRestrictionsSO roomRestrictions;

    [Header("Corridor templates")]
    [SerializeField] GameObject corridorTemplatePrefab;

    Dictionary<RoomType, GameObject> roomTemplates = new();
    void OnValidate() {
        if (roomTemplates.Count > 0) {
            roomTemplates.Clear();
        }

        foreach (var roomTemplate in _roomTemplatesList) {
            roomTemplates.Add(roomTemplate.type, roomTemplate.template);
        }
    }

    public GameObject GetRoomTemplate(RoomType type = RoomType.Normal) => roomTemplates[type];
    public RoomRestrictionsSO RoomRestrictions => roomRestrictions;
    public GameObject CorridorTemplatePrefab => corridorTemplatePrefab;
}

[Serializable]
struct RoomTemplateEtry {
    public RoomType type;
    public GameObject template;
}
