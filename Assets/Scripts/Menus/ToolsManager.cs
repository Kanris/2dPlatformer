using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsManager : MonoBehaviour {

    #region Singleton
    public static ToolsManager instance;

	// Use this for initialization
	void Start () {
		
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

    public void Load(string scene)
    {
        GameObject.FindWithTag("SceneLoader").GetComponent<LoadScene>().Load(scene);
    }
}
