using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    private void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
    }

    public IEnumerator RespawnPlayer()
    {
        Debug.Log("TODO: Player respawn audio");
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public static void KillObject(GameObject objectToKill)
    {
        //Debug.Log(objectToKill.name);
        Destroy(objectToKill);
        gm.StartCoroutine(gm.RespawnPlayer());
    }



}
