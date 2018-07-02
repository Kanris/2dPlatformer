using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResurectionManager : MonoBehaviour {

    public static ResurectionManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    private GameObject resurectionUI;
    private Text resurectionText;

    private bool isResurectionUIActive;

    private void Start()
    {
        resurectionUI = transform.GetChild(0).gameObject;

        if (resurectionUI == null)
        {
            Debug.LogError("ResurectionManager: can't find UI child");
        }

        resurectionText = resurectionUI.GetComponentInChildren<Text>();

        if (resurectionText == null)
        {
            Debug.LogError("ResurectionManager: Can't find text in child");
        }

        isResurectionUIActive = false;
    }

    public void ShowHideResurectionUI()
    {
        isResurectionUIActive = !isResurectionUIActive;
        resurectionUI.SetActive(isResurectionUIActive);

        if (isResurectionUIActive)
        {
            Time.timeScale = 0f;
            resurectionText.text = "Do you want to use Resirection stone (" + PlayerStats.ResurectionStones + ")?";   
        }
    }

    public void Use()
    {
        Time.timeScale = 1f;

        PlayerStats.ResurectionStones--;
        GameMaster.gm.StartCoroutine(GameMaster.gm.RespawnPlayer());
        ShowHideResurectionUI();
    }

    public void DontUse()
    {
        Time.timeScale = 1f;

        GameMaster.gm.GameOver();
    }
}
