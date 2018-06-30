using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2;

	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false;

    private float m_spriteWidth = 0f;
    private Camera m_mainCamera;
    private Transform m_myTransform;

	private void Awake()
	{
		m_mainCamera = Camera.main;
		m_myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		var spriteRenderer = GetComponent<SpriteRenderer>();
		m_spriteWidth = spriteRenderer.sprite.bounds.size.x; //width of sprite
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasALeftBuddy | !hasARightBuddy)
		{
			float camHorizontalExtend = m_mainCamera.orthographicSize * Screen.width / Screen.height;
			float edgeVisiblePositionRight = (m_myTransform.position.x + m_spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (m_myTransform.position.x - m_spriteWidth / 2) + camHorizontalExtend;
   
			if (m_mainCamera.transform.position.x >= edgeVisiblePositionRight - offsetX & !hasARightBuddy)
			{
				CreateNewBuddy(true);
				hasARightBuddy = true;
			}
			else if (m_mainCamera.transform.position.x <= edgeVisiblePositionLeft + offsetX & !hasALeftBuddy)
			{
				CreateNewBuddy(false);
				hasALeftBuddy = true;
			}
		}
	}
    
    void CreateNewBuddy(bool isNeedBuddyOnTheRight)
	{
		var rightOrLeft = isNeedBuddyOnTheRight ? 1 : -1;
        //new position for a new buddy
		var newPosition = new Vector3( m_myTransform.position.x + m_spriteWidth * rightOrLeft, m_myTransform.position.y, m_myTransform.position.z );

		Transform newBuddy = Instantiate(m_myTransform, newPosition, m_myTransform.rotation) as Transform;

		if (reverseScale)
		{
			newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = m_myTransform.parent;

		if (isNeedBuddyOnTheRight)
		{
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		} else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}
}
