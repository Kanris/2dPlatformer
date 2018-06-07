using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 0;

    private PauseMenu pauseMenu;

    private void Start()
    {
        pauseMenu = PauseMenu.pm;
    }

    // Update is called once per frame
    void Update () {

        if (!pauseMenu.IsGamePause)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();

            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);       
        }
	}
}
