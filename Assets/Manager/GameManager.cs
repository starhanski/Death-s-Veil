using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private int numberOfSpawns = 4;
    [SerializeField]
    private Transform spawnPoint;

    public GameObject gato;

    [SerializeField]
    private float spawnTime = 10f;
    private float lastSpawnTime = float.NegativeInfinity;

    private void Start()
    {

    }
    private void Update()
    {
        CheckRespawn();
    }
    private void CheckRespawn()
    {
        if (numberOfSpawns > 0)
        {
            if (Time.time >= lastSpawnTime + spawnTime)
            {
                SpawnEnemy();
                lastSpawnTime = Time.time;
                numberOfSpawns--;
            }
        }

    }
    public void SpawnEnemy()
    {
        Instantiate(gato, spawnPoint);
    }
}
