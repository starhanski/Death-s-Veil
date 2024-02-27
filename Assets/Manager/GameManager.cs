using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float respawnTime;

    private float respawnStartTime;

    private bool respawn;

    private CinemachineVirtualCamera CVC;
    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        CheckRespawn();
    }
    private void CheckRespawn()
    {
        if (Time.time >= respawnStartTime + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
    public void Respawn()
    {
        respawnStartTime = Time.time;
        respawn = true;
    }
}
