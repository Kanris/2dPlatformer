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

    public int LifeCount = 2;
    public Transform LifeGUI;
    public Transform lifeImage;
    private Transform[] lifesLeft;

    public GameObject announcer;

    private AudioManager audioManager;
    public string spawnSoundName;
    public string LevelMusic;
    public string LevelName;

    [HideInInspector]
    public delegate void VoidDelegate();
    public GameObject enemiesSpawnPrefab;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        InitializeLifeGUI();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        if (audioManager == null)
            Debug.LogError("GameMaster: Can't found AudioManager");

        if ( !string.IsNullOrEmpty(LevelName))
        {
            if (enemiesSpawnPrefab != null)
                StartCoroutine(DisplayAnnouncerMessage(LevelName, 2f, () => enemiesSpawnPrefab.SetActive(true)));
            else
                StartCoroutine(DisplayAnnouncerMessage(LevelName, 2f));
        }
    }

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound(spawnSoundName);
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        var spawnEffect = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(spawnEffect, 2.5f);
    }

    public void PlayerIsDead()
    {
        if (LifeCount > 0)
        {
            StartCoroutine(RespawnPlayer());
            LifeCount--;
            ChangeLifeGUI();

            string result = LifeCount > 1 ? "Lifes" : "Life";
            var announcerMessage = LifeCount + " " + result + " left";
            StartCoroutine(DisplayAnnouncerMessage(announcerMessage, 3f));
        }
        else
        {
            gm.StartCoroutine(gm.DisplayAnnouncerMessage("Game Over", 10f));
            StartCoroutine(LoadScene("MainMenu", 4.5f));
        }
    }

    public IEnumerator LoadScene(string scene, float time)
    {
        yield return new WaitForSeconds(time);

        (FindObjectOfType(typeof(LoadScene)) as LoadScene).Load(scene);
    }

    private void InitializeLifeGUI()
    {
        if (lifeImage != null)
        {
            lifesLeft = new Transform[LifeCount];

            for (int index = 0, offsetX = 0; index < LifeCount; index++)
            {
                lifesLeft[index] = Instantiate(lifeImage, LifeGUI.transform);
                lifesLeft[index].position = new Vector3(lifesLeft[index].position.x - offsetX, lifesLeft[index].position.y);
                offsetX += 18;
            }   
        }
    }

    private void ChangeLifeGUI()
    {
        if (lifeImage != null)
            Destroy(lifesLeft[LifeCount].gameObject);
    }

    public IEnumerator DisplayAnnouncerMessage(string message, float duration, VoidDelegate delayFunc = null)
    {
        announcer.SetActive(true);
        announcer.GetComponentInChildren<Text>().text = message;

        yield return new WaitForSeconds(duration);

        announcer.SetActive(false);

        if (delayFunc != null)
        {
            delayFunc();
        }
    }

}
