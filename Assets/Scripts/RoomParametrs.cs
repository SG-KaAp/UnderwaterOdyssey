using UnityEngine;
[CreateAssetMenu(fileName = "Room")]
public class RoomParametrs : ScriptableObject
{
    [field: SerializeField] public Vector3 NextSpawnPoint { get; private set; }
    [field: SerializeField] public GameObject RoomPrefab { get; private set; }
}
