using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private List<RoomParametrs> objects;
    [SerializeField] private int minRooms = 3;
    [SerializeField] private int maxRooms = 6;

    private void Start()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < Random.Range(minRooms, maxRooms); i++)
        {
            RoomParametrs room = objects[Random.Range(0, objects.Count)];
            var obj = Instantiate(room.RoomPrefab,pos,new Quaternion());
            pos = pos + room.NextSpawnPoint;
        }
    }
}
