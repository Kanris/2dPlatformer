using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static void KillObject(GameObject objectToKill)
    {
        //Debug.Log(objectToKill.name);
        Destroy(objectToKill);
    }

}
