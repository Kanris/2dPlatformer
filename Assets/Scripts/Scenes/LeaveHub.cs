using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveHub : MonoBehaviour {

    public string SceneToLoad;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var sceneLoader = (FindObjectOfType(typeof(LoadScene)) as LoadScene);

            if (sceneLoader != null)
                sceneLoader.Load(SceneToLoad);
            else
                Debug.LogError("LeaveHub: Can't find LoadScene");
        }
    }
}
