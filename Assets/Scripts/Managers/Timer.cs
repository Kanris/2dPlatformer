using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float TimeLeft = 30f;

    private Text m_timerText;

    private void Start()
    {
        InitializeTimerText();
    }

    void InitializeTimerText()
    {
        m_timerText = gameObject.GetComponentInChildren<Text>();

        if (m_timerText == null)
        {
            Debug.LogError("Timer: Can't find Text in children component");
        }
    }


    // Update is called once per frame
    void Update () {

        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;

            DisplayTimeLeft();

            if (TimeLeft <= 0f)
            {
                (FindObjectOfType(typeof(GameMaster)) as GameMaster).GameOver();
            }   
        }
	}

    void DisplayTimeLeft()
    {
        m_timerText.text = "Time left: " + ConvertTimeLeft();
    }

    string ConvertTimeLeft()
    {
        return (int)(TimeLeft / 60) + ":" + (TimeLeft % 60).ToString("00");
    }
}
