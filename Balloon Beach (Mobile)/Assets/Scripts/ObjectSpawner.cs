using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] TrianglePrefabs;
    public GameObject Coin;
    Vector3 spawnObstaclePosition;

    // Update is called once per frame
    void Update()
    {
        float distToHorizon = Vector3.Distance(Player.gameObject.transform.position, spawnObstaclePosition);
        if(distToHorizon < 120)
        {
            SpawnCoin();
            SpawnTriangle();
        }
    }

    private void SpawnCoin()
    {
        float[] x = new float[] { -2f, -1.5f, -1f, -0.5f, 0f, 0.5f, 2f, 1.5f, 1f };
        Vector3 temp = spawnObstaclePosition;
        temp.x += x[Random.Range(0, x.Length)];
        temp.z -= Random.Range(5f, 25f);
        Instantiate(Coin, temp, Quaternion.identity);
    }

    private void SpawnTriangle()
    {
        spawnObstaclePosition = new Vector3(0, 0, spawnObstaclePosition.z + 30);
        Instantiate(TrianglePrefabs[(Random.Range(0, TrianglePrefabs.Length))], spawnObstaclePosition, Quaternion.identity);
    }
}
