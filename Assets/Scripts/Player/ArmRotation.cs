using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 0;
    private PauseMenu pauseMenu;

    [HideInInspector]
    public Transform parentTransform;

    private void Start()
    {
        pauseMenu = PauseMenu.pm;
        parentTransform = transform.parent.gameObject.transform;

        if (parentTransform == null)
        {
            Debug.LogError("ArmRotation: can't find parentTransform");
        }
    }

    // Update is called once per frame
    void Update () {

        if (!pauseMenu.IsGamePause)
        {
            Vector3 difference = Vector3.zero;

            if (parentTransform.localScale == new Vector3(-1, 1, 1))
                difference = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); //left
            else
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // right;

            difference.Normalize();

            var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);

            if (rotationZ <= 95f & rotationZ >= -95f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);
            }     
        }
	}
}
