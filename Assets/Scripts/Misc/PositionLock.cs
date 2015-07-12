using UnityEngine;
using System.Collections;

public class PositionLock : MonoBehaviour {

    public Transform otherTransform;
	
	void LateUpdate () {
        transform.position = otherTransform.position;
	}
}
