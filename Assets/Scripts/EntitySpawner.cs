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

    public Transform goPlayerCharParent;
    public Transform goEnemyParent;

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
        SpawnAllPlayerChar();
        SpawnEnemies();
    }

    public void SpawnAllPlayerChar()
    {
        for (int i = 0; i < playerCharList.Capacity; ++i)
        {
            GameObject playerChar = Instantiate(playerCharList[i], playerSpawnLocation[i], Quaternion.identity);
            playerChar.transform.parent = goPlayerCharParent;

            playerChar.transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyList.Capacity; ++i)
        {
            GameObject enemy = Instantiate(enemyList[i], enemySpawnLocation[i], Quaternion.identity);
            enemy.transform.parent = goEnemyParent;

            enemy.transform.Rotate(new Vector3(0, -90, 0));
        }
    }

    public void SpawnEnemies(int _enemyType)
    {
        for (int i = 0; i < enemyList.Capacity; ++i)
        {
            GameObject enemy = Instantiate(enemyList[i], enemySpawnLocation[i], Quaternion.identity);
            enemy.transform.parent = goEnemyParent;

            enemy.transform.Rotate(new Vector3(0, -90, 0));
        }
    }
}
