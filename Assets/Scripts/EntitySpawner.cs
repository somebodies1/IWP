using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private const int maxPlayerChar = 2;
    private const int maxEnemyChar = 2;

    private Dictionary<int, Vector3> playerSpawnLocation = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> enemySpawnLocation = new Dictionary<int, Vector3>();

    public List<GameObject> playerCharList;
    public List<GameObject> enemyList;

    void Start()
    {
        //Player spawn location
        for (int i = 0; i < maxPlayerChar; ++i)
        {
            playerSpawnLocation.Add(i, new Vector3(-3, 0, -5 + (i * -1.5f)));
        }

        //Enemy spawn location
        for (int i = 0; i < maxEnemyChar; ++i)
        {
            enemySpawnLocation.Add(i, new Vector3(3, 0, -5 + (i * -1.5f)));
        }


        for (int i = 0; i < playerCharList.Capacity; ++i)
        {
            Instantiate(playerCharList[i], new Vector3(0, 0, 0), Quaternion.identity);
        }
        
    }
}
