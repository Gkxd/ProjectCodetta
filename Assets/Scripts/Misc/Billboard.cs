using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Transform headTransform;

    void Start() {
        if (headTransform == null) {
            headTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

	void Update () {
        transform.LookAt(headTransform.position);
	}
}
