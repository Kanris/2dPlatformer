using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.7f;
    public Transform spawnPrefab;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
    }

    public IEnumerator RespawnPlayer()
    {
        GetComponent<AudioSource>().Play();
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        var spawnEffect = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(spawnEffect, 2.5f);
    }

    public static void KillObject(GameObject objectToKill)
    {
        //Debug.Log(objectToKill.name);
        Destroy(objectToKill);

        if (objectToKill.tag == "Player")
            gm.StartCoroutine(gm.RespawnPlayer());
    }



}
