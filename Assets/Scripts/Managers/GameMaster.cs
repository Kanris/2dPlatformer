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

    public static int LivesCount = 2;
    public Transform LivesGUI;
    public Transform liveImage;
    private Transform[] livesLeft;

    public GameObject announcer;

    private AudioManager audioManager;
    public string spawnSoundName;
    public string LevelMusic;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        InitializeLiveGUI();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        audioManager.PlaySound(LevelMusic);

        if (audioManager == null)
            Debug.LogError("GameMaster: Can't found AudioManager");
    }

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound(spawnSoundName);
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        var spawnEffect = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(spawnEffect, 2.5f);
    }

    public static void KillObject(GameObject objectToKill)
    {
        if (objectToKill.tag == "Player")
        {
            (FindObjectOfType(typeof(WeaponChange)) as WeaponChange).ResetWeaponGUI();
            gm.audioManager.PlaySound("DeathVoice");
            gm.PlayerIsDead();
        }

        if (objectToKill.tag == "Enemy")
        {
            var sound = objectToKill.GetComponent<EnemyAI>().stats.DeathSound;
            gm.audioManager.PlaySound(sound);
        }
        Debug.LogError(objectToKill.name);
        Destroy(objectToKill);

        var spawnEffect = Instantiate(gm.spawnPrefab, objectToKill.transform.position, objectToKill.transform.rotation).gameObject;
        Destroy(spawnEffect, 2.5f);
    }

    private void PlayerIsDead()
    {
        if (LivesCount > 0)
        {
            StartCoroutine(RespawnPlayer());
            LivesCount--;
            ChangeLiveGui();

            string result = LivesCount > 1 ? "Lifes" : "Life";
            var announcerMessage = LivesCount + " " + result + " left";
            StartCoroutine(DisplayAnnouncerMessage(announcerMessage, 3f));
        }
        else
        {
            gm.StartCoroutine(gm.DisplayAnnouncerMessage("Game Over", 10f));
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

    public IEnumerator DisplayAnnouncerMessage(string message, float duration)
    {
        announcer.SetActive(true);
        announcer.GetComponentInChildren<Text>().text = message;

        yield return new WaitForSeconds(duration);

        announcer.SetActive(false);
    }

}
