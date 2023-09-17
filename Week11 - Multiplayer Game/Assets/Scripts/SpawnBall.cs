using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    //Spawn: 35 50  //Cord X: -2f 2f | Cord Z:
    public float minXCord, maxXCord, minZCord, maxZCord, YCord;
    private float spawnCount; //Random.Range(35, 50);
    public GameObject barrier;

    private void Update()
    {
        
    }

    void SpawnBarrier()
    {
            Vector3 spawnPosition = new Vector3(Random.Range(minXCord, maxXCord), YCord, Random.Range(minZCord, maxZCord));
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(barrier, spawnPosition, spawnRotation);
    }

}
