using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Transform headTransform;
	
	void Update () {
        transform.LookAt(headTransform.position);
	}
}
