using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRespawner : MonoBehaviour
{
    public float respawnPosition;
    public Transform player;
    public float timer;
    public float respawnTime;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= respawnTime)
        {
            timer = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, respawnPosition + player.position.z);
        }
    }
}
