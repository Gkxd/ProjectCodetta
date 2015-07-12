using UnityEngine;
using System.Collections;

public class RotateScroll : MonoBehaviour {
	public Vector2 scrollSpeed;
	public Vector3 rotateSpeed;


	void Update () {
		Vector2 s = scrollSpeed * Time.time;
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = s;
	
		Vector3 r = rotateSpeed * Time.deltaTime;
		transform.Rotate(r);
	}
}
