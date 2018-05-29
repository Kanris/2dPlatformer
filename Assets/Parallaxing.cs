using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds; //array of all the transfortms to be parallaxed
	private float[] parallaxScales; //the proportion of the camera's movement to move the backgrounds by
	public float smoothing = 1f; // how smooth the parallax is going to be (needs to be above 0)

	private Transform cam; //main camera
	private Vector3 previousCamPos; // the position of the camera in the previous frame

	private void Awake()
	{
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;
		parallaxScales = new float[backgrounds.Length];

		for (int index = 0; index < backgrounds.Length; index++)
        {
            parallaxScales[index] = backgrounds[index].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		ApplyParallax(); //applying parallax effect to the backgrounds
	}

    void ApplyParallax()
	{
		for (int index = 0; index < backgrounds.Length; index++)
		{
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[index];
			float backgroundTargetPosX = backgrounds[index].position.x + parallax;

			var backgroundTargetPos = 
				new Vector3(backgroundTargetPosX, backgrounds[index].position.y, backgrounds[index].position.z); //get new position for background
            
			backgrounds[index].position = Vector3.Lerp(backgrounds[index].position, backgroundTargetPos, smoothing * Time.deltaTime); //set new position for background
		}

		previousCamPos = cam.position;
	}
}
