using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyWaveTracker : MonoBehaviour
{
    //kazdy prefab przeciwnika musi to miec okok
    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }
     
    }
}