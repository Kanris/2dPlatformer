using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    private Transform[] m_backgrounds; //array of all the transfortms to be parallaxed
    private float[] m_paralaxScaling; //the proportion of the camera's movement to move the backgrounds by
	public float smoothing = 1f; // how smooth the parallax is going to be (needs to be above 0)

    private Transform m_mainCamera; //main camera
    private Vector3 m_previousCamPos; // the position of the camera in the previous frame

	private void Awake()
	{
		m_mainCamera = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
        InitializeBackgrounds();

		m_previousCamPos = m_mainCamera.position;
		m_paralaxScaling = new float[m_backgrounds.Length];

		for (int index = 0; index < m_backgrounds.Length; index++)
        {
            m_paralaxScaling[index] = m_backgrounds[index].position.z * -1;
        }
	}

    void InitializeBackgrounds()
    {
        var parallaxingList = GameObject.FindGameObjectsWithTag("Parallaxing");
        m_backgrounds = new Transform[parallaxingList.Length];

        for (int index = 0; index < m_backgrounds.Length; index++)
        {
            m_backgrounds[index] = parallaxingList[index].transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
		ApplyParallax(); //applying parallax effect to the backgrounds
	}

    void ApplyParallax()
	{
		for (int index = 0; index < m_backgrounds.Length; index++)
		{
			float parallax = (m_previousCamPos.x - m_mainCamera.position.x) * m_paralaxScaling[index];
			float backgroundTargetPosX = m_backgrounds[index].position.x + parallax;

			var backgroundTargetPos = 
				new Vector3(backgroundTargetPosX, m_backgrounds[index].position.y, m_backgrounds[index].position.z); //get new position for background
            
			m_backgrounds[index].position = Vector3.Lerp(m_backgrounds[index].position, backgroundTargetPos, smoothing * Time.deltaTime); //set new position for background
		}

		m_previousCamPos = m_mainCamera.position;
	}
}
