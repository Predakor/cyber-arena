using Scripts.Player;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Room : MonoBehaviour, IRoomModule
{
    [field: SerializeField] public RoomNode Node { get; private set; }

    public bool IsPreloaded { get; set; }

    public event Action<Player> OnRoomEnter;
    public event Action<Player> OnRoomExit;
    public event Action<Player> OnPlayerNearby;
    public event Action<Player> OnPlayerFaraway;

    public void Init(RoomNode roomNode)
    {
        Node = roomNode;
        float roomSize = roomNode.Data.GetRoomWorldSize() * 0.8f;
        GetComponent<BoxCollider>().size = new(roomSize, 6, roomSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            HandlePlayerEnter(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            HandlePlayerExit(player);
        }
    }

    public void HandlePlayerEnter(Player player)
    {
        OnRoomEnter?.Invoke(player);
        foreach (var neighbour in Node.GetNeighbours())
        {
            if (neighbour is RoomNode node)
            {
                node
                    .GetComponentInChildren<Room>()
                    .HandlePlayerNearby(player);
            }
        }


    }

    public void HandlePlayerExit(Player player)
    {
        OnRoomExit?.Invoke(player);

        //wait for new room and unload all nearby nodes
        //that are not in new room nearby nodes
    }

    public void HandlePlayerNearby(Player player)
    {
        Debug.Log("Player Nearby", this);
        OnPlayerNearby?.Invoke(player);
    }

    public void HandlePlayerFaraway(Player player)
    {
        return;
        //not implemented
        OnPlayerFaraway?.Invoke(player);
    }
}
