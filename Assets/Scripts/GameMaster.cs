using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.7f;
    public Transform spawnPrefab;

    public static int LivesCount = 4;
    public Transform LivesGUI;
    public Transform liveImage;
    private Transform[] livesLeft;

    public Text announcer;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        InitializeLiveGUI();
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
        Destroy(objectToKill);

        if (objectToKill.tag == "Player")
        {
            if (LivesCount > 0)
            {
                gm.StartCoroutine(gm.RespawnPlayer());
                LivesCount--;
                gm.ChangeLiveGui();

                var announcerMessage = LivesCount + " Lives left";
                gm.StartCoroutine(gm.DisplayAnnouncerMessage(announcerMessage, 3f));
            }
        }
    }

    private void InitializeLiveGUI()
    {
        livesLeft = new Transform[LivesCount];

        for (int index = 0, offsetX = 0; index < LivesCount; index++)
        {
            livesLeft[index] = Instantiate(liveImage, LivesGUI.transform);
            livesLeft[index].position = new Vector3(livesLeft[index].position.x - offsetX, livesLeft[index].position.y);
            offsetX += 18;
        }
    }

    private void ChangeLiveGui()
    {
        Destroy(livesLeft[LivesCount].gameObject);
    }

    private IEnumerator DisplayAnnouncerMessage(string message, float duration)
    {
        announcer.text = message;

        yield return new WaitForSeconds(duration);

        announcer.text = string.Empty;
    }

}
