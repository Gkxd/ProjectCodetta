using UnityEngine;
using System.Collections;

public class MouseCameraController : MonoBehaviour {

    private float yaw;
    private float pitch;

	// Use this for initialization
	void Start () {
        GetComponent<CardboardHead>().enabled = false;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -70, 70);

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
	}
}
