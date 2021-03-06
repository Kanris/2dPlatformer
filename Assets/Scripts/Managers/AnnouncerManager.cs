﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncerManager : MonoBehaviour {

    private GameObject announcer;

    [HideInInspector]
    public delegate void VoidDelegate();

    [System.Serializable]
    public class DisplayMessage
    {
        public string message;
        public float duration;
        public VoidDelegate delayFunc = null;

        public DisplayMessage(string message, float duration, VoidDelegate delayFunc = null)
        {
            this.message = message;
            this.duration = duration;
            this.delayFunc = delayFunc;
        }
    }

    private Queue<DisplayMessage> messages;

    public int MessagesCount = 0;

    public static AnnouncerManager instance;

    #region singletone
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

        InitializeMessagesQueue();

        InitalizeAnnouncer();
    }
    #endregion

    private void InitializeMessagesQueue()
    {
        messages = new Queue<DisplayMessage>();
    }

    private void InitalizeAnnouncer()
    {
        announcer = transform.GetChild(0).gameObject;

        if (announcer == null)
            Debug.LogError("GameMaster: Can't find announcer on scene");
        else
        {
            announcer.SetActive(false);
        }
    }

    public void DisplayAnnouncerMessage(string message, float duration, VoidDelegate delayFunc = null)
    {
        messages.Enqueue(new DisplayMessage(message, duration, delayFunc));
        MessagesCount = messages.Count;

        if (!announcer.activeSelf)
        {
            StartCoroutine(DisplayAnnouncerMessage());
        }
    }

    public IEnumerator DisplayAnnouncerMessage()
    {
        var displayMessage = messages.Dequeue();

        MessagesCount = messages.Count;
        announcer.SetActive(true);
        announcer.GetComponentInChildren<Text>().text = displayMessage.message;

        yield return new WaitForSeconds(displayMessage.duration);

        if (displayMessage.delayFunc != null)
            displayMessage.delayFunc();
        
        announcer.SetActive(false);

        if (messages.Count > 0)
            yield return DisplayAnnouncerMessage();
    }
}
