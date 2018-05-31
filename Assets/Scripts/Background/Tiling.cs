using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2;

	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false;

	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;

	private void Awake()
	{
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		var spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.sprite.bounds.size.x; //width of sprite
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasALeftBuddy | !hasARightBuddy)
		{
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;
   
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX & !hasARightBuddy)
			{
				CreateNewBuddy(true);
				hasARightBuddy = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX & !hasALeftBuddy)
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
		var newPosition = new Vector3( myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z );

		Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

		if (reverseScale)
		{
			newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = myTransform.parent;

		if (isNeedBuddyOnTheRight)
		{
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		} else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}
}
