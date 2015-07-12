using UnityEngine;
using System.Collections;

public class YawLock : MonoBehaviour {

    public Transform otherTransform;
	
	// Update is called once per frame
	void Update () {
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.y = otherTransform.eulerAngles.y;
        transform.eulerAngles = eulerAngles;
	}
}
