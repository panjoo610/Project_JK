using UnityEngine;
using System.Collections;

public class RotateSample : MonoBehaviour
{	
	void Start()
    {
		iTween.RotateBy(gameObject, iTween.Hash("y", .85, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .8));
	}
}

