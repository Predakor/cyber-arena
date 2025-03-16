using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_Restrictions_SO", menuName = "Level Generation/Room/Restrictions")]
public class RoomRestrictionsSO : ScriptableObject {

    [SerializeField] List<Restriction> restrictions;

    Dictionary<RoomSize, List<int>> _sizeToSides = new();

    public List<int> GetAllowedSides(RoomSize size) => _sizeToSides[size];

    void OnValidate() {
        if (_sizeToSides.Count > 0) {
            _sizeToSides.Clear();
        }

        foreach (Restriction restriction in restrictions) {
            List<int> sideList = restriction.sides.Split(',')
                .Select(s => int.Parse(s.Trim()))
                .ToList();

            _sizeToSides.Add(restriction.roomSize, sideList);
        }
    }

    [Serializable]
    struct Restriction {
        public RoomSize roomSize;
        public string sides;
    }
}
